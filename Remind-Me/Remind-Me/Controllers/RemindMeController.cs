using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindMe.Controllers;
using Microsoft.AspNetCore.Mvc;
using RemindMe.ViewModels;
using Bandwidth.Net;
using Bandwidth.Net.Api;
using RemindMe.Models;
using RemindMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.ObjectModel;
using System.Collections;
using System.Dynamic;
using Hangfire;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace RemindMe.Controllers
{
    public class RemindMeController : Controller
    {
        //every controller accessing data bases must have these lines at the top

        // test of branch

        private readonly RemindMeDbContext context;

        public RemindMeController(RemindMeDbContext dbContext)
        {
            context = dbContext;
        }

        //
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterUser()
        {
            RegisterUserViewModel newUser = new RegisterUserViewModel();
            return View(newUser);
        }

        

        [HttpPost]
        public IActionResult RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            
            //check to see if the user name entered already exists

            User checkUserName = context.User.FirstOrDefault(u => u.Username == registerUserViewModel.Username);

            if (checkUserName != null)
            {
                ViewBag.userNameExists = "This user name already exists.  Please select another user name";
                ViewBag.colon = ":";
                return View(registerUserViewModel);
            }
            if (ModelState.IsValid)
            {
                /* temporarily hide google calendar input -  will deal with google calendar in release 2
                User newUser = new User(registerUserViewModel.Username, 
                                         registerUserViewModel.Password, 
                                         registerUserViewModel.GCalEmail, 
                                         registerUserViewModel.GCalEmailPassword, 
                                         registerUserViewModel.CellPhoneNumber);
                */
                User newUser = new User(registerUserViewModel.Username, 
                                       registerUserViewModel.Password, 
                                       "dummy gcal email", 
                                       "dummy gcal email password", 
                                       registerUserViewModel.CellPhoneNumber);
                context.User.Add(newUser);
                context.SaveChanges();
                ViewBag.User = registerUserViewModel;

                HttpContext.Session.SetString("Username", newUser.Username);
                HttpContext.Session.SetInt32("ID", newUser.ID);
                HttpContext.Session.SetString("CellPhone", newUser.CellPhoneNumber);
                HttpContext.Session.SetString("CellPhoneNumber", newUser.CellPhoneNumber);
                return View("UserHomePage", newUser);

            }
            return View(registerUserViewModel);
        }

        public IActionResult UserLogin()
        {
            
            UserLoginViewModel returningUser = new UserLoginViewModel();
            return View(returningUser);
        }
        [HttpPost]
        public IActionResult Userlogin(UserLoginViewModel userLoginViewModel)
        {
           
            //  check to see if the user name exists

            try
            {
                User checkUserLogInUserName = context.User.Single(u => u.Username == userLoginViewModel.Username);
            }
            catch (InvalidOperationException)
            {


                ViewBag.userNameNotFound = "User Name was not found";
                return View(userLoginViewModel);
            }
            User checkUserLogInInfo = context.User.Single(u => u.Username == userLoginViewModel.Username);

            if (ModelState.IsValid)
            {
                if (checkUserLogInInfo.Password != userLoginViewModel.Password)
                {
                    ViewBag.userPasswordNotCorrect = "The password entered is invalid";
                    return View(userLoginViewModel);
                }

                HttpContext.Session.SetString("Username", checkUserLogInInfo.Username);  //capture Username for use elsewhere in this app
                HttpContext.Session.SetInt32("ID", checkUserLogInInfo.ID);  //capture ID for use elsewhere in this app
                return View("UserHomePage", checkUserLogInInfo);
            }

            return View(userLoginViewModel);

        }

        [HttpPost]
        public IActionResult UserloginWithGoogleId(string googleId)
        {

            //  check to see if the user name exists

            try
            {
                User checkUserLogInUserName = context.User.Single(u => u.Username == googleId);
            }
            catch (InvalidOperationException)
            {
                ViewBag.userNameNotFound = "User Name was not found";
                return RedirectToAction("UserLogin");
            }

            User checkUserLogInInfo = context.User.Single(u => u.Username == googleId);

            HttpContext.Session.SetString("Username", checkUserLogInInfo.Username);  //capture Username for use elsewhere in this app
            HttpContext.Session.SetInt32("ID", checkUserLogInInfo.ID);  //capture ID for use elsewhere in this app

            return View("UserHomePage", checkUserLogInInfo);
            
        }


        public IActionResult ScheduleEventsAndReminders()
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            //create the ViewModel with the list of repeat frequency options 
            // and reminder time options))
            ScheduleEventsAndRemindersViewModel scheduleEventsAndReminder =
            new ScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList(), context.ReminderTimes.ToList());
           
            //get user's cell phone number

            User currentUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.formattedUserCellPhone = Convert.ToInt64(currentUser.CellPhoneNumber).ToString("(###) ###-####");
            
            //set cell phone number to users cell ohone number
            scheduleEventsAndReminder.UserCellPhoneNumber = currentUser.CellPhoneNumber;
            
            return View(scheduleEventsAndReminder);
        }

        [HttpPost]
        public IActionResult ScheduleEventsAndReminders(ScheduleEventsAndRemindersViewModel newEventAndReminder)

        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            //check to make sure all reminders are scheduled in the same  year

            string start = newEventAndReminder.RecurringReminderStartAlertDate.ToString("yyyy");
            string end = newEventAndReminder.RecurringReminderLastAlertDate.Date.ToString("yyyy");
            bool remindersInSameYear = false;
            ViewBag.calendarYearError = "";

            if (start == end)
            {
                remindersInSameYear = true;

                if (ModelState.IsValid)
                {
                    //  get the Repeat Frequency Selected by the User
                    ReminderRepeatFrequencies newReminderRepeatFrequency =
                        context.ReminderRepeatFrequencies.Single(c => c.ID == newEventAndReminder.RepeatFrequencyNameID);

                    // save the name of the repeat frequency so it can be sent in the confirmation text message

                    string repeatFreqNameUserSelected = newReminderRepeatFrequency.RepeatFrequencyName;

                    User newUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));

                    //get the ReminderTimes the user selected
                    ReminderTimes firstTimeSelected =
                    context.ReminderTimes.FirstOrDefault(id => id.ID == newEventAndReminder.ReminderTimesID);

                    //check to see if user chose "Do No Select" for the second daily reminder
                    // if the user did, then a context statement will throw
                    //an exception since we don't have "Do Not Schedule" stored
                    // in the ReminderTimes table

                    ReminderTimes secondTimeSelected = new ReminderTimes();
                    if (newEventAndReminder.ReminderTimes2ID != 0)
                    {
                        secondTimeSelected = context.ReminderTimes.FirstOrDefault(id => id.ID == newEventAndReminder.ReminderTimes2ID);
                    }
                    else
                    {
                        secondTimeSelected.ReminderTimesName = "Not Scheduled";
                    }
                    //create the RecurringReminder record

                    RecurringReminders newRecurringReminder = new
                        RecurringReminders(newEventAndReminder.RecurringEventName,
                                           newEventAndReminder.RecurringEventDescription,
                                           newEventAndReminder.RecurringEventDate.Date,
                                           newEventAndReminder.RecurringReminderStartAlertDate.Date,
                                           newEventAndReminder.RecurringReminderLastAlertDate.Date,
                                           newReminderRepeatFrequency,
                                           firstTimeSelected.ReminderTimesName,
                                           secondTimeSelected.ReminderTimesName,
                                           newEventAndReminder.UserCellPhoneNumber);

                    newRecurringReminder.User = newUser;

                    context.RecurringReminders.Add(newRecurringReminder);

                    int newRRID = newRecurringReminder.ID; //capture the ID of the new recurring rmeinder for use with the SendTimes records

                    //create the appropriate SendReminder records for the First and Second Alert Times

                    //  get the time the user selected id as assigned
                    // in the AssignSendRemindersTimeFrameID class

                    AssignSendRemindersTimeFrameID timeSelectedFirst = new AssignSendRemindersTimeFrameID();
                    int firstTimeSelectedAsInteger = timeSelectedFirst.DetermineSendReminderTime
                        (
                          firstTimeSelected.ReminderTimesName
                        );

                    //if a second daily reminder was not scheduled the return 
                    //from this next call will be -99 and no second reminder will be scheduled
                      
                    AssignSendRemindersTimeFrameID timeSelectedSecond = new AssignSendRemindersTimeFrameID();
                       int secondTimeSelectedAsInteger = timeSelectedSecond.DetermineSendReminderTime
                            (
                              secondTimeSelected.ReminderTimesName
                            );

                    // a couple of checks to see if the user entered times that fall into
                    //the same six hour period and to see if the user entered a second time
                    //that is earlier than the first time

                    //first check to see if the second time selected is earlier than
                    //the first time selected

                    // save the user selected firstTime info in case we have to swap it with the secondTime
                    string saveFirstTime = firstTimeSelected.ReminderTimesName;
                    int savefirstTimeSelectedAsInteger = firstTimeSelectedAsInteger;
                    string saveSecondTime = secondTimeSelected.ReminderTimesName;

                    // get the earliest of the two times selected by the user
                    saveFirstTime =  ReturnEarliestTime(firstTimeSelected.ReminderTimesName, secondTimeSelected.ReminderTimesName);

                    // if secondTime is earlier than first time then do the  swap
                    if (firstTimeSelected.ReminderTimesName != saveFirstTime)
                    {
                        saveFirstTime = secondTimeSelected.ReminderTimesName;
                        firstTimeSelectedAsInteger = secondTimeSelectedAsInteger;
                        saveSecondTime = firstTimeSelected.ReminderTimesName;
                        secondTimeSelectedAsInteger = savefirstTimeSelectedAsInteger;
                    }


                    // make sure you save the alert times in the right order
                    // since you did a check of which time is earlier

                    newRecurringReminder.RecurringReminderFirstAlertTime = saveFirstTime;
                    newRecurringReminder.RecurringReminderSecondAlertTime = saveSecondTime;
                    
                    // save the new event and reminder and ReminderTimes to the data base

                    context.SaveChanges();

                    //save the new Recurring Reminder for entry into the SendRemindersXXXXXX times
                    //Table

                    RecurringReminders savedNewRecurringReminder = newRecurringReminder;

                    // save the times selected by the user into the SendReminder XXXX times
                    // and return the IDs of the SendReminder Times for including in the
                    //RecurringReminder record

                    SaveSendReminderTimes
                                           (
                                          firstTimeSelectedAsInteger, 
                                          saveFirstTime,
                                          secondTimeSelectedAsInteger,
                                          saveSecondTime, 
                                          ref savedNewRecurringReminder
                                          );


                    ViewBag.eventDate = newEventAndReminder.RecurringEventDate.Date;
                   
                    // Since adding the event/reminder was successful we will
                    //send a confirming text to the user.
                    // first get the text credentials
                    var textInfo = (context.TextInfo.Where(id => id.ID > 0).ToList()); // there is only 1 record in this table
                    
                    // now send the confirmation text
                    SendTextMessagesController sendConfirmation = new SendTextMessagesController();
                    sendConfirmation.SendTextMessage
                        (
                        newEventAndReminder.UserCellPhoneNumber,
                        newEventAndReminder.RecurringEventName,
                        newEventAndReminder.RecurringEventDate.Date,
                        newEventAndReminder.RecurringEventDescription,
                        newEventAndReminder.RecurringReminderStartAlertDate.Date,
                        newEventAndReminder.RecurringReminderLastAlertDate.Date,
                        textInfo,
                        repeatFreqNameUserSelected
                        );

                    return View("RecurringEventsAndReminders", newRecurringReminder);
                }
            }
            // if we get here the model is not valid or the reminders 
            // are not in the same year. So we need to create
            // a new viewmodel to include the repeat frequency choices
            // and populate it with the data the user entered
            if (remindersInSameYear == false)
            {
                ViewBag.calendarYearError = "Both the Start and Stop Dates for " +
                                             "Reminders must be in the same year"; 
            }
            ScheduleEventsAndRemindersViewModel scheduleEventsAndReminder =
                new ScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList(), context.ReminderTimes.ToList());

            scheduleEventsAndReminder.RecurringEventName = newEventAndReminder.RecurringEventName;
            scheduleEventsAndReminder.RecurringEventDescription = newEventAndReminder.RecurringEventDescription;
            scheduleEventsAndReminder.RecurringEventDate = newEventAndReminder.RecurringEventDate;
            scheduleEventsAndReminder.RecurringReminderStartAlertDate = newEventAndReminder.RecurringReminderStartAlertDate;
            scheduleEventsAndReminder.RecurringReminderLastAlertDate = newEventAndReminder.RecurringReminderLastAlertDate;
            scheduleEventsAndReminder.UserCellPhoneNumber = newEventAndReminder.UserCellPhoneNumber;
            
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.formattedUserCellPhone = Convert.ToInt64(newEventAndReminder.UserCellPhoneNumber).ToString("(###) ###-####");
            
            return View(scheduleEventsAndReminder);
        }

        public ActionResult SingleUserRecurringEventsAndReminders()
        {

            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");

            dynamic userRecurringReminders = (from ch in context.RecurringReminders
                                              join
                                              ca in context.User
                                              on ch.UserId equals ca.ID
                                              where ca.ID == HttpContext.Session.GetInt32("ID")
                                              select new
                                              {
                                                  ch.RecurringReminderName,
                                                  ch.RecurringReminderDescription,
                                                  ch.RecurringEventDate,
                                                  ch.RecuringReminderCreateDate,
                                                  ch.RecurringReminderStartAlertDate,
                                                  ch.RecurringReminderLastAlertDate,
                                                  ch.RecurringReminderFirstAlertTime,
                                                  ch.RecurringReminderSecondAlertTime,
                                                  ch.RepeatFrequencyName,
                                                  ch.UserCellPhoneNumber,
                                                  ch.RecurringReminderDateAndTimeLastAlertSent,
                                                  ch.ID
                                              }).AsEnumerable().Select(c => c.ToExpando());
            // we need to convert from an Enumerable of Expando Objects
            //to a List in order to sort the items
            List<dynamic> sortedUserRecurringReminders = new List<dynamic>();

            
            // sort the userRecurringReminders
            //first create an empty list of ExpandoObjects
            IList<ExpandoObject> expandoUserRecurringReminders = new List<ExpandoObject>();

            // next populate that list 
            foreach (dynamic rr in userRecurringReminders)
            {
                ExpandoObject rrExpando = rr; // convert each recurringreminder from dynamic type to ExpandoObject type
                expandoUserRecurringReminders.Add(rrExpando); //then add them to the list
            }

            //sort the items and add them to the List
            foreach (dynamic reminder in expandoUserRecurringReminders.
                 OrderBy(x => ((IDictionary<string, object>)x)["RecurringReminderName"]))
            {
                sortedUserRecurringReminders.Add(reminder);
            }
            ViewBag.numOfReminders = sortedUserRecurringReminders.Count();
            return View(sortedUserRecurringReminders);
            

        }

        public IActionResult UserLogout()
        {
            HttpContext.Session.SetString("Username", "");
            ViewBag.userloggedout = true;
            return View("Index");
        }

        public IActionResult UserHomePage()
        {
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            // if Session("Username") has not been set yet we need to catch the error
            try
            {
                User currentUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));
                return View(currentUser);
            }
            catch (System.InvalidOperationException )
            {
                return View("Index");
            }

        }

        [HttpGet]
        public IActionResult EditEventsAndReminders(int recurringReminderId)
        {

            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");

            // get the recurring reminder to be edited

            RecurringReminders editRecurringReminder = context.RecurringReminders.
                FirstOrDefault(id => id.ID == recurringReminderId);

            //get the reminder times scheduled from the recurringreminder record just retrieved
            //then get the ID from the RemiderTimes table so we can show that time
            //in the view for the user to edit

            ReminderTimes firstTime = context.ReminderTimes.FirstOrDefault
                (
                  time => time.ReminderTimesName == editRecurringReminder.RecurringReminderFirstAlertTime
                );

            int secondTime = 0;

            // see if there was a second time schedued
            if (editRecurringReminder.RecurringReminderSecondAlertTime != "Not Scheduled")
            {
                
                ReminderTimes thereIsASecondTime = context.ReminderTimes.FirstOrDefault
                    (
                      time => time.ReminderTimesName == editRecurringReminder.RecurringReminderSecondAlertTime
                    );
                secondTime = thereIsASecondTime.ID;
            }

            //create the ViewModel with the list of repeat frequency options ))
            EditScheduleEventsAndRemindersViewModel editEventsAndReminder = new
            EditScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList(), context.ReminderTimes.ToList())
            {
                // populate the fields for the reminder to be edited
                RecurringEventName = editRecurringReminder.RecurringReminderName,
                RecurringEventDescription = editRecurringReminder.RecurringReminderDescription,
                RecurringEventDate = editRecurringReminder.RecurringEventDate,
                RecurringReminderStartAlertDate = editRecurringReminder.RecurringReminderStartAlertDate,
                RecurringReminderLastAlertDate = editRecurringReminder.RecurringReminderLastAlertDate,
                UserCellPhoneNumber = editRecurringReminder.UserCellPhoneNumber,
                RepeatFrequencyNameID = editRecurringReminder.RepeatFrequencyNameID,
                ReminderTimesID = firstTime.ID,
                ReminderTimes2ID = secondTime,

                UserId = editRecurringReminder.UserId
            };
           
           
             
            // save the ID for use in the Post method
            HttpContext.Session.SetString("recurringReminderId", recurringReminderId.ToString());
            ViewBag.formattedUserCellPhone = Convert.ToInt64(editRecurringReminder.UserCellPhoneNumber).ToString("(###) ###-####");
            return View(editEventsAndReminder);
        }

        [HttpPost]
        public IActionResult EditEventsAndReminders(EditScheduleEventsAndRemindersViewModel recurringReminder)
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            //check to make sure all reminders are scheduled in the current calendar year

            string start = recurringReminder.RecurringReminderStartAlertDate.ToString("yyyy");
            string end = recurringReminder.RecurringReminderLastAlertDate.Date.ToString("yyyy");
            bool remindersInSameYear = false;
            ViewBag.calendarYearError = "";

            if (start == end)
            {
                remindersInSameYear = true;
                if (ModelState.IsValid)
                {
                    // get the Repeat Frequency selected by the user
                    ReminderRepeatFrequencies newReminderRepeatFrequency =
                                         context.ReminderRepeatFrequencies.
                                         Single(c => c.ID == recurringReminder.RepeatFrequencyNameID);

                    // get User info for the recurring reminder
                    User newUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));

                    //Get the recurring reminder record the user wants to modify 
                    //we will delete that record once we add the revised recurring reminder
                    //with the data entered by the user in the edit form

                    RecurringReminders editRR = context.RecurringReminders.
                        Single(id => id.ID.Equals(Int32.Parse(HttpContext.Session.GetString("recurringReminderId"))));

                    // delete the sendremindertimexxxxx records existing prior to the edit

                    DeleteSendReminderTimeRecords(editRR);

                    //get the ReminderTimes the user selected
                    ReminderTimes firstTimeSelected =
                    context.ReminderTimes.FirstOrDefault(id => id.ID == recurringReminder.ReminderTimesID);

                    //check to see if user chose "Do No Select" for the second daily reminder
                    // if the user did, then a context statement will throw
                    //an exception since we don't have "Do Not Schedule" stored
                    // in the ReminderTimes table

                    ReminderTimes secondTimeSelected = new ReminderTimes();
                    if (recurringReminder.ReminderTimes2ID != 0)
                    {
                        secondTimeSelected = context.ReminderTimes.FirstOrDefault(id => id.ID == recurringReminder.ReminderTimes2ID);
                    }
                    else
                    {
                        secondTimeSelected.ReminderTimesName = "Not Scheduled";
                    }
                    

                   

                    // create new Recurring Reminder with data input from the user from the edit form

                    RecurringReminders revisedRR = new RecurringReminders
                        (

                                    recurringReminder.RecurringEventName,
                                    recurringReminder.RecurringEventDescription,
                                    recurringReminder.RecurringEventDate,
                                    recurringReminder.RecurringReminderStartAlertDate,
                                    recurringReminder.RecurringReminderLastAlertDate,
                                    newReminderRepeatFrequency,
                                    firstTimeSelected.ReminderTimesName,
                                    secondTimeSelected.ReminderTimesName,
                                    recurringReminder.UserCellPhoneNumber

                       );

                    revisedRR.User = newUser;

                    

                    //save the recurring reminder with the revised data 
                    context.RecurringReminders.Add(revisedRR);
                    
                    int newRRID = revisedRR.ID; //capture the ID of the edited recurring reminder for use with the SendTimes records
                     
                    //create the appropriate SendReminder records for the First and Second Alert Times

                    //  get the time the user selected id as assigned
                    // in the AssignSendRemindersTimeFrameID class
                    
                    AssignSendRemindersTimeFrameID timeSelectedFirst = new AssignSendRemindersTimeFrameID();
                    int firstTimeSelectedAsInteger = timeSelectedFirst.DetermineSendReminderTime
                        (
                          firstTimeSelected.ReminderTimesName
                        );

                    //if a second daily reminder was not scheduled the return 
                    //from this next call will be -99 and no second reminder will be scheduled
                      
                    AssignSendRemindersTimeFrameID timeSelectedSecond = new AssignSendRemindersTimeFrameID();
                       int secondTimeSelectedAsInteger = timeSelectedSecond.DetermineSendReminderTime
                            (
                              secondTimeSelected.ReminderTimesName
                            );
                   
                    // a couple of checks to see if the user entered times that fall into
                    //the same six hour period and to see if the user entered a second time
                    //that is earlier than the first time

                    //first check to see if the second time selected is earlier than
                    //the first time selected

                    // save the user selected firstTime info in case we have to swap it with the secondTime
                    string saveFirstTime = firstTimeSelected.ReminderTimesName;
                    int savefirstTimeSelectedAsInteger = firstTimeSelectedAsInteger;
                    string saveSecondTime = secondTimeSelected.ReminderTimesName;
                    
                    // get the earliest of the two times selected by the user
                    saveFirstTime =  ReturnEarliestTime(firstTimeSelected.ReminderTimesName, secondTimeSelected.ReminderTimesName);

                    // if secondTime is earlier than first time then do the  swap
                    if (firstTimeSelected.ReminderTimesName != saveFirstTime)
                    {
                        saveFirstTime = secondTimeSelected.ReminderTimesName;
                        firstTimeSelectedAsInteger = secondTimeSelectedAsInteger;
                        saveSecondTime = firstTimeSelected.ReminderTimesName;
                        secondTimeSelectedAsInteger = savefirstTimeSelectedAsInteger;
                    }

                    // make sure you save the alert times in the right order
                    // since you did a check of which time is earlier

                    revisedRR.RecurringReminderFirstAlertTime = saveFirstTime;
                    revisedRR.RecurringReminderSecondAlertTime = saveSecondTime;
                    
                    // save the new event and reminder and ReminderTimes to the data base

                    context.SaveChanges();

                    //save the new Recurring Reminder for entry into the SendRemindersXXXXXX times
                    //Table

                    RecurringReminders savedNewRecurringReminder = revisedRR;

                    // save the times selected by the user into the SendReminder XXXX times
                    // and return the IDs of the SendReminder Times for includino in the
                    //RecurringReminder record

                    SaveSendReminderTimes
                                           (
                                          firstTimeSelectedAsInteger, 
                                          saveFirstTime,
                                          secondTimeSelectedAsInteger,
                                          saveSecondTime, 
                                          ref savedNewRecurringReminder
                                          );

                    // Since editing the event/reminder was successful we will
                    //send a confirming text to the user.
                    // first get the text credentials
                    var textInfo = (context.TextInfo.Where(id => id.ID > 0).ToList()); // there is only 1 record in this table

                    // save the name of the repeat frequency so it can be sent in the confirmaiton text message

                    string repeatFreqNameUserSelected = newReminderRepeatFrequency.RepeatFrequencyName;



                    SendTextMessagesController sendConfirmation = new SendTextMessagesController();
                    sendConfirmation.SendTextMessage
                        (
                        recurringReminder.UserCellPhoneNumber,
                        recurringReminder.RecurringEventName,
                        recurringReminder.RecurringEventDate.Date,
                        recurringReminder.RecurringEventDescription,
                        recurringReminder.RecurringReminderStartAlertDate.Date,
                        recurringReminder.RecurringReminderLastAlertDate.Date,
                        textInfo,
                        repeatFreqNameUserSelected
                        );
                    // remove the original recurring reminder the user wanted to edit
                    context.RecurringReminders.Remove(editRR);
                    context.SaveChanges();
                    ViewBag.eventDate = recurringReminder.RecurringEventDate.Date;
                    return View("RecurringEventsAndReminders", revisedRR);
                }
            }
            // if we get here the model is not valid or the reminders 
            // are not in the current year. So we need to create
            // a new viewmodel to include the repeat frequency choices and ReminderTimes choices
            // and populate it with the data the user entered
            if (remindersInSameYear == false)
            {
                ViewBag.calendarYearError = "Both the Start and Stop Dates for " +
                                             "Reminders must be in the current year";
            }
            EditScheduleEventsAndRemindersViewModel editEventsAndReminder = new
            EditScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList(), context.ReminderTimes.ToList())
            {
                // populate the fields for the reminder to be edited
                RecurringEventName = recurringReminder.RecurringEventName,
                RecurringEventDescription = recurringReminder.RecurringEventDescription,
                RecurringEventDate = recurringReminder.RecurringEventDate,
                RecurringReminderStartAlertDate = recurringReminder.RecurringReminderStartAlertDate,
                RecurringReminderLastAlertDate = recurringReminder.RecurringReminderLastAlertDate,
                UserCellPhoneNumber = recurringReminder.UserCellPhoneNumber,
                RepeatFrequencyNameID = recurringReminder.RepeatFrequencyNameID,
                ReminderTimes = recurringReminder.ReminderTimes,
                ReminderTimes2 = recurringReminder.ReminderTimes2,
                UserId = recurringReminder.UserId
            };
            ViewBag.formattedUserCellPhone = Convert.ToInt64(recurringReminder.UserCellPhoneNumber).ToString("(###) ###-####");
            return View(editEventsAndReminder);
        }

        [HttpGet]
        public IActionResult DeleteEventsAndReminders(int recurringReminderId = -1)
        {
            

            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            // get all of the users events/recurring reminders for 
            //multiple event/reminder deletions

            if (recurringReminderId == -1)
            {
                var userRecurringReminders = context.RecurringReminders.
                    Where(un => un.UserId == HttpContext.Session.GetInt32("ID")).ToList();

                //sort the list of reminders
                userRecurringReminders.Sort((p, q) => p.RecurringReminderName.CompareTo(q.RecurringReminderName));


                //remove ambiguity in re List type: 
                //cast from var to List<RecurringReminders>

                List<RecurringReminders> recurringReminders = new List<RecurringReminders>();

                foreach (RecurringReminders rr in userRecurringReminders)
                {
                    recurringReminders.Add(rr);
                }
                ViewBag.recurringReminders = recurringReminders;
            }
            //retrieve single event/reminder to be deleted
            // search for the UserId and ID in recurring reminder table 
            // associated with the event/reminder the user wants to delete
            else
            {
                var userRecurringReminders = context.RecurringReminders.
                      Where(un => un.UserId == HttpContext.Session.GetInt32("ID")).
                      Where(id => id.ID == recurringReminderId).ToList();
                
                //remove ambiguity in re List type: 
                //cast from var to List<RecurringReminders>

                List<RecurringReminders> recurringReminders = new List<RecurringReminders>();

                foreach (RecurringReminders rr in userRecurringReminders)
                {
                    recurringReminders.Add(rr);
                }
                ViewBag.recurringReminders = recurringReminders;
            }
            ViewBag.title = "Remove Events/Reminders";
            
          
            return View();

        }

        [HttpPost]
        public IActionResult DeleteEventsAndReminders(int[] recurringReminderId)
        {
            // check to see if the user has logged in

            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            
            //capture recurring events/reminders to delete in a list to 
            //show the user to confirm before actually deleting any 
            // recurring events/reminders

            List<RecurringReminders> rrToDelete = new List<RecurringReminders>(recurringReminderId.Count());
            int i = 0; // Index for rrToDelete populating
            
                foreach (int id in recurringReminderId)
                {
                    // rrToDelete[i] = context.RecurringReminders.FirstOrDefault(d => d.ID == id);
                    rrToDelete.Add(context.RecurringReminders.FirstOrDefault(d => d.ID == id));
                    ++i;
                }

            //rrToDelete.OrderBy(name => name.RecurringReminderName);
            //Sort the list by Recurring Reminder Name
            //rrToDelete.Sort((p, q) => p.RecurringReminderName.CompareTo(q.RecurringReminderName));
            ViewBag.deleteTheseReminders = rrToDelete;
            return View("ConfirmDeleteEventsAndRemindersBeforeDeleting", rrToDelete);
        
        }
       
        public IActionResult ConfirmDeleteEventsAndRemindersBeforeDeleting(List<RecurringReminders> rrToDelete)
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            // display the  recurring reminder the user wants to delete
            
            ViewBag.deleteTheseReminders = rrToDelete;
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteEventsAndRemindersBeforeDeleting(int[] recurringReminderId)
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            
            // remove the  recurring reminder(s) the user wants to delete

            List<RecurringReminders> rrToDelete = new List<RecurringReminders>(recurringReminderId.Count());
            int i = 0; // Index for rrToDelete populating

            foreach (int id in recurringReminderId)
            {
                rrToDelete.Add(context.RecurringReminders.FirstOrDefault(d => d.ID == id));
                ++i;
            }

            foreach (RecurringReminders rr in rrToDelete)
            {
                DeleteSendReminderTimeRecords(rr);
                context.RecurringReminders.Remove(rr);
                context.SaveChanges();
            }

            return RedirectToAction("UserHomePage");
            
        }
        //method to add ReminderTimes to the database
        // a security device - this method will only work on 06/20/2018
        // in order to run it on another date I will have to change 06/20/2018
        // to the date I want to run it on
        
        [HttpGet]
        public IActionResult AddReminderTimes()
        {
            // a security device - this method will only work on 06/20/2018
            // in order to run it on another date I will have to change 06/20/2018
            // to the date I want to run it on
            if ((DateTime.Now.ToString("MM/dd/yyyy")).Equals("07/04/2018"))
            {

                AddReminderTimesViewModel addReminderTimes = new AddReminderTimesViewModel();
                return View(addReminderTimes);
            }
            // if it is not the date in the if statement above, do notmpermit
            // changes to these data items and return the Index view
            return View("Index");
        }
        [HttpPost]
        public IActionResult AddReminderTimes(AddReminderTimesViewModel reminderTime)
        {
            if (ModelState.IsValid)
            {

                if (reminderTime.ReminderTimesName != "")
                {
                    ReminderTimes newReminderTime = new ReminderTimes();
                    newReminderTime.ReminderTimesName = reminderTime.ReminderTimesName;
                    
                    context.ReminderTimes.Add(newReminderTime);
                    context.SaveChanges();
                }
                
                
                return RedirectToAction("AddReminderTimes");
            }
            return View(reminderTime);
        }


        //method to add event frequencies 
        //and Text Credentials to the database- ie Annually, Once, etc

        [HttpGet]
        public IActionResult AddTextCredentialsAndFrequencies()
        {
            // a security device - this method will only work on 06/20/2018
            // in order to run it on another date I will have to change 06/20/2018
            // to the date I want to run it on
            if ((DateTime.Now.ToString("MM/dd/yyyy")).Equals("07/03/2018"))
            {

                AddTextCredentialsAndFrequenciesViewModel addTextCredentialsAndEventFrequencies = new AddTextCredentialsAndFrequenciesViewModel();
                return View(addTextCredentialsAndEventFrequencies);
            }
            // if it is not the date in the if statement above, do notmpermit
            // changes to these data items and return the Index view
            return View("Index");
        }

        [HttpPost]
        public IActionResult AddTextCredentialsAndFrequencies(AddTextCredentialsAndFrequenciesViewModel textAndFrequencies)
        {
          if (ModelState.IsValid)
            {
                
                if (textAndFrequencies.RepeatFrequencyNameOne != "")
                {
                    ReminderRepeatFrequencies newFreq = new ReminderRepeatFrequencies();
                    newFreq.RepeatFrequencyName = textAndFrequencies.RepeatFrequencyNameOne;
                    context.ReminderRepeatFrequencies.Add(newFreq);
                }
                if (textAndFrequencies.RepeatFrequencyNameTwo != "")
                {
                    ReminderRepeatFrequencies newFreq = new ReminderRepeatFrequencies();
                    newFreq.RepeatFrequencyName = textAndFrequencies.RepeatFrequencyNameTwo;
                    context.ReminderRepeatFrequencies.Add(newFreq);
                }
                
                    TextInfo newTextInfo = new TextInfo();
                    newTextInfo.TextFrom = textAndFrequencies.TextFrom;
                    newTextInfo.TextSecret = textAndFrequencies.TextSecret;
                    newTextInfo.TextUserId = textAndFrequencies.TextUserId;
                    newTextInfo.TextToken = textAndFrequencies.TextToken;

                context.TextInfo.Add(newTextInfo);
                context.SaveChanges();
                return View("../Home/About");
            }
            return View("Index");
        }

        //we want to make sure the first time is earlier than the second time
        //if it isn't we will swap the values of first time and second time so that
        //when they are displayed to the user the first time shown is earlier than
        //the second time

        public string ReturnEarliestTime(string firstTime, string secondTime)
        {
            
            string firstTimeIsAmOrPm =
                        firstTime.Substring(firstTime.Length - 2);
                     
            string secondTimeIsAmOrPm =
               secondTime.Substring(secondTime.Length - 2);

            // check if one of times in AM and the other is PM

            if (firstTimeIsAmOrPm == "AM" && secondTimeIsAmOrPm == "PM")
            {
                return firstTime;
            }

            if (firstTimeIsAmOrPm == "PM" && secondTimeIsAmOrPm == "AM")
            {
                return secondTime;
            }

            //evaluate since both times are AM or PM

            if (firstTime.Substring(0, 5).CompareTo(secondTime.Substring(0, 5)) <=0) 
                {
                return firstTime;
                }

            // if we get here then secondTime is earlier than firstTime
            return secondTime;
        }

        // this method takes the AssignSendRemindersTimeFrameID and saves the data to
        // the corresponding table in the data base

        public void SaveSendReminderTimes 
                                           (
                                           int firstSendRemindersTimeFrameID, 
                                           string firstReminderTime,
                                           int secondSendReminderTimeFrameID,
                                           string secondReminderTime,
                                           ref RecurringReminders theRecurringReminder
                                           )
         {
            
            // save the first time selected
            switch (firstSendRemindersTimeFrameID)
            {
                case 0:
                    SendRemindersMidnightToFiveAm midnight = new SendRemindersMidnightToFiveAm()
                    {
                        TimeToSendReminderMTFAM = firstReminderTime,
                        RecurringReminderId = theRecurringReminder.ID,
                        FirstOrSecondReminderOfTheDay = "First"
                    };

                    context.SendRemindersMidnightToFiveAm.Add(midnight);
                    context.SaveChanges();  // must do a save so SQL will assign an ID
                    theRecurringReminder.TimeToSendReminderMTFAMID = midnight.ID;
                    break;

                case 1:
                    SendRemindersSixAmToElevenAm sixToElevenAm = new SendRemindersSixAmToElevenAm()
                    {
                        TimeToSendReminderSTEAM = firstReminderTime,
                        RecurringReminderId = theRecurringReminder.ID,
                        FirstOrSecondReminderOfTheDay = "First"
                    };

                    context.SendRemindersSixAmToElevenAm.Add(sixToElevenAm);
                    context.SaveChanges();  // must do a save so SQL will assign an ID
                    theRecurringReminder.TimeToSendReminderSTEAMID = sixToElevenAm.ID;
                    break;

                case 2:
                    SendRemindersNoonToFivePm noonToFivePm = new SendRemindersNoonToFivePm()
                    {
                        TimeToSendReminderNTFPM = firstReminderTime,
                        RecurringReminderId = theRecurringReminder.ID,
                        FirstOrSecondReminderOfTheDay = "First"
                    };

                    context.SendRemindersNoonToFivePm.Add(noonToFivePm);
                    context.SaveChanges();  // must do a save so SQL will assign an ID
                    theRecurringReminder.TimeToSendReminderNTFPMID = noonToFivePm.ID;
                    break;

                case 3:
                    SendRemindersSixPmToElevenPm sixPmToElevenPm = new SendRemindersSixPmToElevenPm()
                    {
                        TimeToSendReminderSTEPM = firstReminderTime,
                        RecurringReminderId = theRecurringReminder.ID,
                        FirstOrSecondReminderOfTheDay = "First"
                    };

                    context.SendRemindersSixPmToElevenPm.Add(sixPmToElevenPm);
                    context.SaveChanges(); // must do a save so SQL will assign an ID
                    theRecurringReminder.TimeToSendReminderSTEPMID = sixPmToElevenPm.ID;
                    break;

            }

            // Second Time Selected
            // get the time the user selected as integer
            // first check to see if the user selected two times from the same group

            if (firstSendRemindersTimeFrameID != secondSendReminderTimeFrameID)
            {
                switch (secondSendReminderTimeFrameID)
                {
                    case 0:
                        SendRemindersMidnightToFiveAm midnight = new SendRemindersMidnightToFiveAm()
                        {
                            TimeToSendReminderMTFAM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };


                        context.SendRemindersMidnightToFiveAm.Add(midnight);
                        context.SaveChanges(); // must do a save so SQL will assign an ID
                        theRecurringReminder.TimeToSendReminderMTFAMID = midnight.ID;
                        break;

                    case 1:
                        SendRemindersSixAmToElevenAm sixToElevenAm = new SendRemindersSixAmToElevenAm()
                        {
                            TimeToSendReminderSTEAM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersSixAmToElevenAm.Add(sixToElevenAm);
                        context.SaveChanges();  // must do a save so SQL will assign an ID
                        theRecurringReminder.TimeToSendReminderSTEAMID = sixToElevenAm.ID;
                        break;

                    case 2:
                        SendRemindersNoonToFivePm noonToFivePm = new SendRemindersNoonToFivePm()
                        {
                            TimeToSendReminderNTFPM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersNoonToFivePm.Add(noonToFivePm);
                        context.SaveChanges(); // must do a save so SQL will assign an ID
                        theRecurringReminder.TimeToSendReminderNTFPMID = noonToFivePm.ID;
                        break;

                    case 3:
                        SendRemindersSixPmToElevenPm sixPmToElevenPm = new SendRemindersSixPmToElevenPm()
                        {
                            TimeToSendReminderSTEPM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };
                        
                        context.SendRemindersSixPmToElevenPm.Add(sixPmToElevenPm);
                        context.SaveChanges();  // must do a save so SQL will assign an ID
                        theRecurringReminder.TimeToSendReminderSTEPMID = sixPmToElevenPm.ID;
                        break;
                }
            }
            // if we get here the user has selected two times in the same
            //timetosendreminderXXXX group
            else
            {
                switch (secondSendReminderTimeFrameID)
                {
                    case 0:
                        SendRemindersMidnightToFiveAm midnight = new SendRemindersMidnightToFiveAm()
                        {
                            TimeToSendReminderMTFAM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersMidnightToFiveAm.Add(midnight);
                        context.SaveChanges();  // must do a save so SQL will assign an ID

                        //since the times to send reminders are in the same group
                        // we save the second time to send reminder info here
                        theRecurringReminder.TimeToSendReminderDuplicatesID = midnight.ID;
                        break;

                    case 1:
                        SendRemindersSixAmToElevenAm sixToElevenAm = new SendRemindersSixAmToElevenAm()
                        {
                            TimeToSendReminderSTEAM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersSixAmToElevenAm.Add(sixToElevenAm);
                        context.SaveChanges();  // must do a save so SQL will assign an ID

                        //since the times to send reminders are in the same group
                        // we save the second time to send reminder ID info here
                        theRecurringReminder.TimeToSendReminderDuplicatesID = sixToElevenAm.ID;
                        break;

                    case 2:
                        SendRemindersNoonToFivePm noonToFivePm = new SendRemindersNoonToFivePm()
                        {
                            TimeToSendReminderNTFPM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersNoonToFivePm.Add(noonToFivePm);
                        context.SaveChanges();  // must do a save so SQL will assign an ID

                        //since the times to send reminders are in the same group
                        // we save the second time to send reminder info here
                        theRecurringReminder.TimeToSendReminderDuplicatesID = noonToFivePm.ID;
                        break;

                    case 3:
                        SendRemindersSixPmToElevenPm sixPmToElevenPm = new SendRemindersSixPmToElevenPm()
                        {
                            TimeToSendReminderSTEPM = secondReminderTime,
                            RecurringReminderId = theRecurringReminder.ID,
                            FirstOrSecondReminderOfTheDay = "Second"
                        };

                        context.SendRemindersSixPmToElevenPm.Add(sixPmToElevenPm);
                        context.SaveChanges();  // must do a save so SQL will assign an ID

                        //since the times to send reminders are in the same group
                        // we save the second time to send reminder info here
                        theRecurringReminder.TimeToSendReminderDuplicatesID = sixPmToElevenPm.ID;

                        break;
                }

            }
            context.SaveChanges();  // must save the recurring reminder now that the send times xxxx have been assigned into it
            
        }

        //This method will delete a sendremindersXXXXXX record 
        //as part of both editing and deleting recurring reminder records

        public void DeleteSendReminderTimeRecords
            (
               RecurringReminders theRecurringReminder
            )

        {
            //retrieve the sendreminderXXXXX IDs from the recurringreminder record passed to this method

            List<int> times = new List<int>();
           
            int numOfTimes = 0;

            if (theRecurringReminder.TimeToSendReminderMTFAMID > 0) { times.Add(0);  numOfTimes += 1; }
            if (theRecurringReminder.TimeToSendReminderSTEAMID > 0) { times.Add(1); numOfTimes += 1; }
            if (theRecurringReminder.TimeToSendReminderNTFPMID > 0) { times.Add(2); numOfTimes += 1; }
            if (theRecurringReminder.TimeToSendReminderSTEPMID > 0) { times.Add(3); numOfTimes += 1; }
            if (theRecurringReminder.TimeToSendReminderDuplicatesID > 0) numOfTimes += 1;

            switch (times[0])
            {
                case 0:
                    
                    SendRemindersMidnightToFiveAm removeMTFAM = context.SendRemindersMidnightToFiveAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderMTFAMID);
                    context.SendRemindersMidnightToFiveAm.Remove(removeMTFAM);
                    context.SaveChanges();   
                    break;

                case 1:
                    SendRemindersSixAmToElevenAm removeSTEAM = context.SendRemindersSixAmToElevenAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderSTEAMID);
                    context.SendRemindersSixAmToElevenAm.Remove(removeSTEAM);
                    context.SaveChanges();   
                    break;
                  
                case 2:
                    SendRemindersNoonToFivePm removeNTFPM = context.SendRemindersNoonToFivePm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderNTFPMID);
                    context.SendRemindersNoonToFivePm.Remove(removeNTFPM);
                    context.SaveChanges();   
                    break;

                case 3:
                    SendRemindersSixPmToElevenPm removeSTEPM = context.SendRemindersSixPmToElevenPm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderSTEPMID);
                    context.SendRemindersSixPmToElevenPm.Remove(removeSTEPM);
                    context.SaveChanges();   
                    break;
            }
            if (times.Count==2)
            {
                switch (times[1])
                {
                    case 0:
                        SendRemindersMidnightToFiveAm removeMTFAM = context.SendRemindersMidnightToFiveAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderMTFAMID);
                        context.SendRemindersMidnightToFiveAm.Remove(removeMTFAM);
                        context.SaveChanges();   
                        break;

                    case 1:
                        SendRemindersSixAmToElevenAm removeSTEAM = context.SendRemindersSixAmToElevenAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderSTEAMID);
                        context.SendRemindersSixAmToElevenAm.Remove(removeSTEAM);
                        context.SaveChanges();   
                        break;

                    case 2:
                        SendRemindersNoonToFivePm removeNTFPM = context.SendRemindersNoonToFivePm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderNTFPMID);
                        context.SendRemindersNoonToFivePm.Remove(removeNTFPM);
                        context.SaveChanges();   
                        break;

                    case 3:
                        SendRemindersSixPmToElevenPm removeSTEPM = context.SendRemindersSixPmToElevenPm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderSTEPMID);
                        context.SendRemindersSixPmToElevenPm.Remove(removeSTEPM);
                        context.SaveChanges();   
                        break;

                }
            }
            //here we will check to see if there was a second time 
            //within the same sendtimesxxxx group as the first time

            if (times.Count == 1 && numOfTimes == 2)
            {
                switch (times[0])
                {
                    case 0:

                        SendRemindersMidnightToFiveAm removeMTFAM = context.SendRemindersMidnightToFiveAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderDuplicatesID);
                        context.SendRemindersMidnightToFiveAm.Remove(removeMTFAM);
                        context.SaveChanges();
                        break;

                    case 1:
                        SendRemindersSixAmToElevenAm removeSTEAM = context.SendRemindersSixAmToElevenAm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderDuplicatesID);
                        context.SendRemindersSixAmToElevenAm.Remove(removeSTEAM);
                        context.SaveChanges();
                        break;

                    case 2:
                        SendRemindersNoonToFivePm removeNTFPM = context.SendRemindersNoonToFivePm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderDuplicatesID);
                        context.SendRemindersNoonToFivePm.Remove(removeNTFPM);
                        context.SaveChanges();
                        break;

                    case 3:
                        SendRemindersSixPmToElevenPm removeSTEPM = context.SendRemindersSixPmToElevenPm.FirstOrDefault(id => id.ID == theRecurringReminder.TimeToSendReminderDuplicatesID);
                        context.SendRemindersSixPmToElevenPm.Remove(removeSTEPM);
                        context.SaveChanges();
                        break;
                }
            }
        }

        // These Methods are called from Startup.cs  - that launches Hangfire background tasks

        public IActionResult LaunchSendRecurringReminderTexts(Object j)
        {
            BackgroundJob.Enqueue(() => GetTheSendReminderTimesToBeChecked());
            return null;
        }

        public IActionResult LaunchResetRecurringReminderDateAndTimeLastAlertSent(Object j)
        {
            BackgroundJob.Enqueue(() => ResetRecurringReminderDateAndTimeLastAlertSent());
            return null;
        }
         
        public void GetTheSendReminderTimesToBeChecked()
        {
            List<int> theReminderIDs = new List<int>();
            string currentTime = DateTime.Now.ToShortTimeString();

            //must adjust currentTime for single digit hours
            if (currentTime.Length == 7) { currentTime = "0" + currentTime; }
            
            //we need to convert the times to 24 hour format for comparisons later on
            //in this method
            
            int hourIn24HourFormatAsInt = Int32.Parse(DateTime.Parse(currentTime).ToString("HH"));

            //save the hour and half hour for selecting reminders to be sent 
            //in the Send Texts Annually and Once methods and AM or PM for
            //use later on in this method
            
            string halfHour = currentTime.Substring(3, 1);
            string hour = currentTime.Substring(0, 2);
            string amOrPM = currentTime.Substring(6, 2);

            string firstOrSecondReminderOfTheDay = "";

            //determine which sendremindertimesxxxx model to use
            //at the current time

            if (hourIn24HourFormatAsInt.CompareTo(0) >= 0 && hourIn24HourFormatAsInt.CompareTo(5) <= 0)
            {
                var checkToSeeIfTheseRemindersShouldBeSent = context.SendRemindersMidnightToFiveAm.
                    Where(ti => ti.TimeToSendReminderMTFAM == currentTime).ToList();
                foreach (var ID in checkToSeeIfTheseRemindersShouldBeSent)
                {
                    if (
                        ID.TimeToSendReminderMTFAM.Substring(0, 2) == hour &&
                        ID.TimeToSendReminderMTFAM.Substring(3, 1) == halfHour &&
                        ID.TimeToSendReminderMTFAM.Substring(6, 2) == amOrPM
                        
                        )
                    {
                        theReminderIDs.Add(ID.RecurringReminderId);
                        firstOrSecondReminderOfTheDay = ID.FirstOrSecondReminderOfTheDay;
                        //sendRemindersTimeFrameID = 1;
                    }
                }
            }

            else if (hourIn24HourFormatAsInt.CompareTo(6) >= 0 && hourIn24HourFormatAsInt.CompareTo(11) <= 0)
            {
                var checkToSeeIfTheseRemindersShouldBeSent = context.SendRemindersSixAmToElevenAm.
                    Where(ti => ti.TimeToSendReminderSTEAM == currentTime).ToList();
                foreach (var ID in checkToSeeIfTheseRemindersShouldBeSent)
                {
                    if (
                        ID.TimeToSendReminderSTEAM.Substring(0, 2) == hour &&
                        ID.TimeToSendReminderSTEAM.Substring(3, 1) == halfHour &&
                        ID.TimeToSendReminderSTEAM.Substring(6, 2) == amOrPM
                        )
                    {
                        theReminderIDs.Add(ID.RecurringReminderId);
                        firstOrSecondReminderOfTheDay = ID.FirstOrSecondReminderOfTheDay;
                        //sendRemindersTimeFrameID = 1;
                    }
                }
            }

            else if (hourIn24HourFormatAsInt.CompareTo(12) >= 0 && hourIn24HourFormatAsInt.CompareTo(17) <= 0)
            {
                var checkToSeeIfTheseRemindersShouldBeSent = context.SendRemindersNoonToFivePm.ToList();
                

                /*
                var checkToSeeIfTheseRemindersShouldBeSent = context.SendRemindersNoonToFivePm.
                    Where(ti => ti.TimeToSendReminderNTFPM == currentTime).ToList();
                */
                foreach (var ID in checkToSeeIfTheseRemindersShouldBeSent)
                {
                    if (
                        ID.TimeToSendReminderNTFPM.Substring(0, 2) == hour &&
                        ID.TimeToSendReminderNTFPM.Substring(3, 1) == halfHour &&
                        ID.TimeToSendReminderNTFPM.Substring(6, 2) == amOrPM
                        )
                    {
                        theReminderIDs.Add(ID.RecurringReminderId);
                        firstOrSecondReminderOfTheDay = ID.FirstOrSecondReminderOfTheDay;
                        //sendRemindersTimeFrameID = 2;
                    }
                }
            }

            else if (hourIn24HourFormatAsInt.CompareTo(18) >= 0 && hourIn24HourFormatAsInt.CompareTo(23) <= 0)
            {
                var checkToSeeIfTheseRemindersShouldBeSent = context.SendRemindersSixPmToElevenPm.
                    Where(ti => ti.TimeToSendReminderSTEPM == currentTime).ToList();
                foreach (var ID in checkToSeeIfTheseRemindersShouldBeSent)
                {
                    if (
                        ID.TimeToSendReminderSTEPM.Substring(0, 2) == hour &&
                        ID.TimeToSendReminderSTEPM.Substring(3, 1) == halfHour &&
                        ID.TimeToSendReminderSTEPM.Substring(6, 2) == amOrPM
                        
                        )
                    {
                        theReminderIDs.Add(ID.RecurringReminderId);
                        firstOrSecondReminderOfTheDay = ID.FirstOrSecondReminderOfTheDay;
                        //sendRemindersTimeFrameID = 3;
                    }
                }
            }
            SendRecurringReminderTextsAnnually(theReminderIDs, firstOrSecondReminderOfTheDay);
            SendRecurringReminderTextsOnce(theReminderIDs, firstOrSecondReminderOfTheDay);
        }
        
        public IActionResult SendRecurringReminderTextsAnnually
            (
            List<int> theReminderIDsToBeChecked,
            string firstOrSecondReminderOfTheDay
            )

        {
            // create a list of the annual reminders that are scheduled to go out today
            
            string today = DateTime.Now.Date.ToString("MM/dd"); // convert today's date to 
                                                                //string for comparison to dates 
                                                                //in recurringreminders
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");

            /*
            var rrDueToday = (context.RecurringReminders.Where
            (
            rr => rr.RepeatFrequencyName.RepeatFrequencyName == "Annually" &&
            today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
            (today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 ||
            today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0) &&
            (DateTime.Now.Date.ToString("MM/dd").CompareTo
                   (rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")
                   ) > 0 ||
                   rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo
                   ("2001") == 0)).ToList()
             );
            */

            // gather those reminders due at the current Time

            RecurringReminders tempy = new RecurringReminders();
            List<RecurringReminders> rrsToBeInspected = new List<RecurringReminders>();
            foreach (int rrID in theReminderIDsToBeChecked)
            {
                // rrsToBeInspected.Add(context.RecurringReminders.First(rr => rr.ID == rrID));

                tempy = (context.RecurringReminders.Include( freq => freq.RepeatFrequencyName).Single(rr => rr.ID == rrID));
                rrsToBeInspected.Add(tempy);
                     
            }

            List<RecurringReminders> rrDueToday = new List<RecurringReminders>();
            //inspect the reminders to see which ones are scheduled to be sent today
            foreach (RecurringReminders rr in rrsToBeInspected)
            {
                if (
                    rr.RepeatFrequencyName.RepeatFrequencyName.ToString() == "Annually" &&
                   today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
                  (today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 ||
                  today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0) ||
                  (rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo
                   ("2001") == 0 && rr.RecurringReminderLastAlertDate.Date.ToString("yyyy").CompareTo(DateTime.Now.Date.Year.ToString()) >= 0))
                {
                 //if this is the First Alert of the Day to be sent
                 
                    if (firstOrSecondReminderOfTheDay == "First" &&
                       
                        (rr.FirstOrSecondAlertOfTheDay == null  || 
                         rr.FirstOrSecondAlertOfTheDay == "Second" ||
                         rr.RecurringReminderSecondAlertTime == "Not Scheduled") &&
                         (today.CompareTo(
                            rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0 ||
                             rr.RecurringReminderDateAndTimeLastAlertSent.ToString() == "01/01")
                       )
                    {
                        rrDueToday.Add(rr);
                        
                    }

                    //if this is the second alert of the day
                    else if (firstOrSecondReminderOfTheDay == "Second" &&
                             (rr.FirstOrSecondAlertOfTheDay == null ||
                             rr.FirstOrSecondAlertOfTheDay == "First" ||
                             today.CompareTo(rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0) 
                            )
                    {
                        rrDueToday.Add(rr);
                    }
                    
                }
            }

            
            //capture the reminders that are scheduled for the current time
            //as per GetTheSendReminderTimesToBeChecked method from above

            List<RecurringReminders> rrDueNow = new List<RecurringReminders>();
            Console.WriteLine("***************");
            Console.WriteLine("theReminderIDsToBeChecked Count = " + theReminderIDsToBeChecked.Count);
            Console.WriteLine("***************");

            //determine which of the reminders in rrDueToday should be sent out now

            foreach (RecurringReminders RR in rrDueToday)
            {  
               if (theReminderIDsToBeChecked.Contains(RR.ID) == true) 
                {
                    rrDueNow.Add(RR);
                }

            }
            
            Console.WriteLine("We are after the var statement");
            Console.WriteLine("Count of Annual Reminders in var rrDueToday: " + rrDueToday.Count());
            Console.WriteLine("Count of Annual Reminders in var rrDueNow: " + rrDueNow.Count());

            //Get TextInfo and populate text parameters//
            string TextId = "";
            string TextToken = "";
            string TextSecret = "";
            string TextFrom = "";

            var textInfo = (context.TextInfo.Where(id => id.ID > 0).ToList()); // there is only 1 record in this table

            foreach (var ti in textInfo)
            {
                TextId = ti.TextUserId;
                TextToken = ti.TextToken;
                TextSecret = ti.TextSecret;
                TextFrom = ti.TextFrom;
            }
            Console.WriteLine("Text ID = See PW Doc");     //  + TextId);
            Console.WriteLine("Text Token = See PW Doc");  // + TextToken);
            Console.WriteLine("Text Secret = See PW Doc");  // + TextSecret);
            Console.WriteLine("Text From = See PW Doc");                //  + TextFrom);
            //
            // send reminders
            foreach (RecurringReminders rr in rrDueNow)
            {
                try
                {
                    Console.WriteLine("We are before the TRY command");
                    Console.WriteLine("Text ID = See PW Doc");
                    Console.WriteLine("Text Token = See PW Doc");
                    Console.WriteLine("Text Secret = See PW Doc");
                    Console.WriteLine("Text From = See PW Doc");
                    Console.WriteLine("RecurringReminderDateAndTimeLastAlertSent.Date = " + rr.RecurringReminderDateAndTimeLastAlertSent.Date);
                    Console.WriteLine("DateTime.Now.Date = " + DateTime.Now.Date);
                    Console.WriteLine("Comparison of above two variables = " + (DateTime.Now > rr.RecurringReminderDateAndTimeLastAlertSent));

                    // format text message
                    string eventDate = rr.RecurringEventDate.ToString("MM/dd"); // Just include the month and day of the event in the text message
                    string textMessage = "From: TheReminderFactory" + "\u2122" + "\r\n" + "Event: " + rr.RecurringReminderName + "\r\n" + "Description: " + rr.RecurringReminderDescription + "\r\nEvent Date: " + eventDate;

                    SendMessage(rr.UserCellPhoneNumber, TextFrom, textMessage, TextId, TextToken, TextSecret).Wait();
                    Console.WriteLine("We are after the TRY command");

                    // Update the current record to reflect the date the latest alert was sent
                    //and identify it as the First or Second alert sent

                    rr.RecurringReminderDateAndTimeLastAlertSent = DateTime.Now;
                    rr.FirstOrSecondAlertOfTheDay = firstOrSecondReminderOfTheDay;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.ReadLine();
                }

            }

            //

            return null;
        }

        public IActionResult SendRecurringReminderTextsOnce
            (
            List<int> theReminderIDsToBeChecked,
            string firstOrSecondReminderOfTheDay
            )

        {
            // create a list of the annual reminders that are scheduled to go out today

            string today = DateTime.Now.Date.ToString("MM/dd"); // convert today's date to 
                                                                //string for comparison to dates 
                                                                //in recurringreminders
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");

            /*
            var rrDueToday = (context.RecurringReminders.Where
            (
            rr => rr.RepeatFrequencyName.RepeatFrequencyName == "Annually" &&
            today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
            (today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 ||
            today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0) &&
            (DateTime.Now.Date.ToString("MM/dd").CompareTo
                   (rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")
                   ) > 0 ||
                   rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo
                   ("2001") == 0)).ToList()
             );
            */

            // gather those reminders due at the current Time

            List<RecurringReminders> rrsToBeInspected = new List<RecurringReminders>();
            foreach (int rrID in theReminderIDsToBeChecked)
            {
                rrsToBeInspected.Add(context.RecurringReminders.First(rr => rr.ID == rrID));
            }

            List<RecurringReminders> rrDueToday = new List<RecurringReminders>();
            //inspect the reminders to see which ones are scheduled to be sent today
            foreach (RecurringReminders rr in rrsToBeInspected)
            {
                if (
                    rr.RepeatFrequencyName.RepeatFrequencyName.ToString() == "Once" &&
                   today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
                  (today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 ||
                  today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0) ||
                  rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo
                   ("2001") == 0)
                {
                    //if this is the First Alert of the Day to be sent

                    if (firstOrSecondReminderOfTheDay == "First" &&

                        (rr.FirstOrSecondAlertOfTheDay == null ||
                         rr.FirstOrSecondAlertOfTheDay == "Second" ||
                         rr.RecurringReminderSecondAlertTime == "Not Scheduled") &&
                         today.CompareTo(
                            rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0
                       )
                    {
                        rrDueToday.Add(rr);

                    }

                    // if this is the second alert of the day
                    else if (firstOrSecondReminderOfTheDay == "Second" &&
                             (rr.FirstOrSecondAlertOfTheDay == null ||
                             rr.FirstOrSecondAlertOfTheDay == "First" ||
                             today.CompareTo(rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0) 
                            )
                    {
                        rrDueToday.Add(rr);
                    }

                }
            }


            //capture the reminders that are scheduled for the current time
            //as per GetTheSendReminderTimesToBeChecked method from above

            List<RecurringReminders> rrDueNow = new List<RecurringReminders>();
            Console.WriteLine("***************");
            Console.WriteLine("theReminderIDsToBeChecked Count = " + theReminderIDsToBeChecked.Count);
            Console.WriteLine("***************");

            //determine which of the reminders in rrDueToday should be sent out now

            foreach (RecurringReminders RR in rrDueToday)
            {
                if (theReminderIDsToBeChecked.Contains(RR.ID) == true)
                {
                    rrDueNow.Add(RR);
                }

            }

            Console.WriteLine("We are after the var statement");
            Console.WriteLine("Count of Annual Reminders in var rrDueToday: " + rrDueToday.Count());
            Console.WriteLine("Count of Annual Reminders in var rrDueNow: " + rrDueNow.Count());

            //Get TextInfo and populate text parameters//
            string TextId = "";
            string TextToken = "";
            string TextSecret = "";
            string TextFrom = "";

            var textInfo = (context.TextInfo.Where(id => id.ID > 0).ToList()); // there is only 1 record in this table

            foreach (var ti in textInfo)
            {
                TextId = ti.TextUserId;
                TextToken = ti.TextToken;
                TextSecret = ti.TextSecret;
                TextFrom = ti.TextFrom;
            }
            Console.WriteLine("Text ID = See PW Doc");     //  + TextId);
            Console.WriteLine("Text Token = See PW Doc");  // + TextToken);
            Console.WriteLine("Text Secret = See PW Doc");  // + TextSecret);
            Console.WriteLine("Text From = See PW Doc");                //  + TextFrom);
            //
            // send reminders
            foreach (RecurringReminders rr in rrDueNow)
            {
                try
                {
                    Console.WriteLine("We are before the TRY command");
                    Console.WriteLine("Text ID = See PW Doc");
                    Console.WriteLine("Text Token = See PW Doc");
                    Console.WriteLine("Text Secret = See PW Doc");
                    Console.WriteLine("Text From = See PW Doc");
                    Console.WriteLine("RecurringReminderDateAndTimeLastAlertSent.Date = " + rr.RecurringReminderDateAndTimeLastAlertSent.Date);
                    Console.WriteLine("DateTime.Now.Date = " + DateTime.Now.Date);
                    Console.WriteLine("Comparison of above two variables = " + (DateTime.Now > rr.RecurringReminderDateAndTimeLastAlertSent));

                    // format text message
                    string eventDate = rr.RecurringEventDate.ToString("MM/dd"); // Just include the month and day of the event in the text message
                    string textMessage = "From: TheReminderFactory" + "\u2122" + "\r\n" + "Event: " + rr.RecurringReminderName + "\r\n" + "Description: " + rr.RecurringReminderDescription + "\r\nEvent Date: " + eventDate;

                    SendMessage(rr.UserCellPhoneNumber, TextFrom, textMessage, TextId, TextToken, TextSecret).Wait();
                    Console.WriteLine("We are after the TRY command");

                    // Update the current record to reflect the date the latest alert was sent
                    //and identifyit as the First or Second alert sent

                    rr.RecurringReminderDateAndTimeLastAlertSent = DateTime.Now;
                    rr.FirstOrSecondAlertOfTheDay = firstOrSecondReminderOfTheDay;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.ReadLine();
                }

            }

            //

            return null;
        }


        //retrieve text info

        private static async Task SendMessage(string to, string from, string text, string textId, string textToken, string textSecret)

        {
            var data = new Bandwidth.Net.Api.MessageData
            {
                To = to, // number receiving text essage
                From = from, //bandwidth number
                Text = text  // reminder
            };

            Console.WriteLine("Text ID = See PW Doc"); //+ textId;
            Console.WriteLine("Text Token = See PW Doc"); // + textToken);
            Console.WriteLine("Text Secret = See PW Doc"); // + textSecret);
            Console.WriteLine("Text From = See PW Doc"); // + from);


            Console.WriteLine("We are before the var client command");
            var client = new Client(textId, textToken, textSecret);
            Console.WriteLine("We are after the var client command");

            Console.WriteLine("We are before the var message command");
            var message = await client.Message.SendAsync(data);
            Console.WriteLine("We are after the var message command");

        }
        public IActionResult ResetRecurringReminderDateAndTimeLastAlertSent()
        {

            Console.WriteLine("Starting to retrieve the records");
            
            //get ID of "Annually" from the database

            var repeatFreq = context.ReminderRepeatFrequencies.Where(rfn => rfn.RepeatFrequencyName == "Annually").ToList();
            
            ReminderRepeatFrequencies annually = repeatFreq[0];  //this is a list with one element
           
            //retireve the recurring reminder records that are "annual" reminders

            var annualRecurringReminders = context.RecurringReminders.Where(rr => rr.RepeatFrequencyNameID == annually.ID).ToList();
            
            Console.WriteLine("Finished retrieving the Annually records");
            Console.WriteLine("Number of Annually records found: " + annualRecurringReminders.Count());
            
            //reset the RecurringReminderDateAndTimeLastAlertSent for annual reminders to 01/01/2001

            foreach (var rr in annualRecurringReminders)
            {
                rr.RecurringReminderDateAndTimeLastAlertSent = Convert.ToDateTime("01/01/2001");
            }
            context.SaveChanges();
            
            return null;
        }
        
    }

}
