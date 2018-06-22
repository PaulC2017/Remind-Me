using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace RemindMe.Controllers
{
    public class RemindMeController : Controller
    {
        //every controller accessing data bases must have these lines at the top

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
            catch (InvalidOperationException e)
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

        
        public IActionResult ScheduleEventsAndReminders()
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }

            //create the ViewModel with the list of repeat frequency options ))
            ScheduleEventsAndRemindersViewModel scheduleEventsAndReminder =
                new ScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList());
            
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

                    // create recurring reminder record

                    //first get the Repeat Frequency Selected by the User
                    ReminderRepeatFrequencies newReminderRepeatFrequency =
                        context.ReminderRepeatFrequencies.Single(c => c.ID == newEventAndReminder.RepeatFrequencyNameID);

                    User newUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));

                    RecurringReminders newRecurringReminder = new
                        RecurringReminders(newEventAndReminder.RecurringEventName,
                                           newEventAndReminder.RecurringEventDescription,
                                           newEventAndReminder.RecurringEventDate.Date,
                                           newEventAndReminder.RecurringReminderStartAlertDate.Date,
                                           newEventAndReminder.RecurringReminderLastAlertDate.Date,
                                           newReminderRepeatFrequency,
                                           newEventAndReminder.UserCellPhoneNumber);

                    newRecurringReminder.User = newUser;

                    context.RecurringReminders.Add(newRecurringReminder);

                    // save the new event and reminder to the data base

                    context.SaveChanges();
                    ViewBag.eventDate = newEventAndReminder.RecurringEventDate.Date;

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
                new ScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList());

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
            catch (System.InvalidOperationException e)
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
            Console.WriteLine("*************");
            Console.WriteLine("editRecurringReminder.RecurringEventDate is " + 
                              editRecurringReminder.RecurringEventDate +
                              " ToString " + editRecurringReminder.RecurringEventDate.ToString());
            Console.WriteLine("*************");
            //create the ViewModel with the list of repeat frequency options ))
            EditScheduleEventsAndRemindersViewModel editEventsAndReminder = new
            EditScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList())
            {
                // populate the fields for the reminder to be edited
                RecurringEventName = editRecurringReminder.RecurringReminderName,
                RecurringEventDescription = editRecurringReminder.RecurringReminderDescription,
                RecurringEventDate = editRecurringReminder.RecurringEventDate,
                RecurringReminderStartAlertDate = editRecurringReminder.RecurringReminderStartAlertDate,
                RecurringReminderLastAlertDate = editRecurringReminder.RecurringReminderLastAlertDate,
                UserCellPhoneNumber = editRecurringReminder.UserCellPhoneNumber,
                RepeatFrequencyNameID = editRecurringReminder.RepeatFrequencyNameID,
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

                    // create new Recurring Reminder with data input from the user from the edit form

                    RecurringReminders revisedRR = new RecurringReminders
                        (

                                    recurringReminder.RecurringEventName,
                                    recurringReminder.RecurringEventDescription,
                                    recurringReminder.RecurringEventDate,
                                    recurringReminder.RecurringReminderStartAlertDate,
                                    recurringReminder.RecurringReminderLastAlertDate,
                                    newReminderRepeatFrequency,
                                    recurringReminder.UserCellPhoneNumber

                       );

                    revisedRR.User = newUser;

                    //save the recurring reminder with ther revised data 
                    context.RecurringReminders.Add(revisedRR);

                    // remove the original recurring reminder the user wanted to edit
                    context.RecurringReminders.Remove(editRR);
                    context.SaveChanges();
                    ViewBag.eventDate = recurringReminder.RecurringEventDate.Date;
                    return View("RecurringEventsAndReminders", revisedRR);
                }
            }
            // if we get here the model is not valid or the reminders 
            // are not in the current year. So we need to create
            // a new viewmodel to include the repeat frequency choices
            // and populate it with the data the user entered
            if (remindersInSameYear == false)
            {
                ViewBag.calendarYearError = "Both the Start and Stop Dates for " +
                                             "Reminders must be in the current year";
            }
            EditScheduleEventsAndRemindersViewModel editEventsAndReminder = new
            EditScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList())
            {
                // populate the fields for the reminder to be edited
                RecurringEventName = recurringReminder.RecurringEventName,
                RecurringEventDescription = recurringReminder.RecurringEventDescription,
                RecurringEventDate = recurringReminder.RecurringEventDate,
                RecurringReminderStartAlertDate = recurringReminder.RecurringReminderStartAlertDate,
                RecurringReminderLastAlertDate = recurringReminder.RecurringReminderLastAlertDate,
                UserCellPhoneNumber = recurringReminder.UserCellPhoneNumber,
                RepeatFrequencyNameID = recurringReminder.RepeatFrequencyNameID,
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
            Console.WriteLine("**************");
            Console.WriteLine("We are in the Delete Post");
            Console.WriteLine("**************");


            //capture recurring events/reminders to delete in a list to 
            //show the user to confirm before actually deleting any 
            // recurring events/reminders

            Console.WriteLine("**************");
            Console.WriteLine("recurringRemindersCount = " + recurringReminderId.Count().ToString());
            Console.WriteLine("**************");

            List<RecurringReminders> rrToDelete = new List<RecurringReminders>(recurringReminderId.Count());
            int i = 0; // Index for rrToDelete populating
            
                foreach (int id in recurringReminderId)
                {
                    // rrToDelete[i] = context.RecurringReminders.FirstOrDefault(d => d.ID == id);
                    rrToDelete.Add(context.RecurringReminders.FirstOrDefault(d => d.ID == id));
                    Console.WriteLine("**************");
                    Console.WriteLine("rrToDelete count = " + rrToDelete.Count.ToString());
                    Console.WriteLine("rrToDelete.Name = " + rrToDelete[i].RecurringReminderName.ToString());
                    Console.WriteLine("i = " + i.ToString());
                    Console.WriteLine("**************");
                    ++i;
                }

            //rrToDelete.OrderBy(name => name.RecurringReminderName);
            //Sort the list by Recurring Reminder Name
            //rrToDelete.Sort((p, q) => p.RecurringReminderName.CompareTo(q.RecurringReminderName));
            ViewBag.deleteTheseReminders = rrToDelete;
            return View("ConfirmDeleteEventsAndRemindersBeforeDeleting", rrToDelete);
        /*
        RecurringReminders deleteRR = context.RecurringReminders.
                Single(id => id.ID.Equals(deleteReminder.recurringReminderId));

            return View("DeleteEventsAndReminders", deleteRR );


        return null;
        */
        }
       
        public IActionResult ConfirmDeleteEventsAndRemindersBeforeDeleting(List<RecurringReminders> rrToDelete)
        {
            // check to see if the user has logged in
            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            // remove the  recurring reminder the user wants to delete
            /*
            foreach (RecurringReminders rr in ViewBag.rrTodelete)
            {
                context.RecurringReminders.Remove(rr);
                context.SaveChanges();
            }
            // return View("UserHomePage", HttpContext.Session.GetString("Username"));
            */
            ViewBag.deleteTheseReminders = rrToDelete;
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteEventsAndRemindersBeforeDeleting(int[] recurringReminderId)
        {
            // remove the  recurring reminder(s) the user wants to delete

            List<RecurringReminders> rrToDelete = new List<RecurringReminders>(recurringReminderId.Count());
            int i = 0; // Index for rrToDelete populating

            foreach (int id in recurringReminderId)
            {
                // rrToDelete[i] = context.RecurringReminders.FirstOrDefault(d => d.ID == id);
                rrToDelete.Add(context.RecurringReminders.FirstOrDefault(d => d.ID == id));
                Console.WriteLine("**************");
                Console.WriteLine("rrToDelete count = " + rrToDelete.Count.ToString());
                Console.WriteLine("rrToDelete.Name = " + rrToDelete[i].RecurringReminderName.ToString());
                Console.WriteLine("i = " + i.ToString());
                Console.WriteLine("**************");
                ++i;
            }

            foreach (RecurringReminders rr in rrToDelete)
            {
                context.RecurringReminders.Remove(rr);
                context.SaveChanges();
            }

            return RedirectToAction("UserHomePage");
            
        }
        //method to add event frequencies 
        //and Text Credentials to the database- ie Annually, Once, etc

        [HttpGet]
        public IActionResult AddTextCredentialsAndFrequencies()
        {
            // a security device - this method will only work on 06/20/2018
            // in order to run it on another date I will have to change 06/20/2018
            // to the date I want to run it on
            if ((DateTime.Now.ToString("MM/dd/yyyy")).Equals("06/20/2018"))
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
        
        // These Methods are called from Startup.cs  - that launches Hangfire background tasks

        public IActionResult LaunchSendRecurringReminderTextsAnnually(Object j)
        {

            BackgroundJob.Enqueue(() => SendRecurringReminderTextsAnnually());

            return null;
        }

        public IActionResult LaunchSendRecurringReminderTextsOnce(Object j)
        {

            BackgroundJob.Enqueue(() => SendRecurringReminderTextsOnce());

            return null;
        }

        public IActionResult LaunchResetRecurringReminderDateAndTimeLastAlertSent(Object j)
        {

            BackgroundJob.Enqueue(() => ResetRecurringReminderDateAndTimeLastAlertSent());

            return null;
        }

      
        public IActionResult SendRecurringReminderTextsAnnually()

        {
            // create a list of the annual reminders that are scheduled to go out today
            
            string today = DateTime.Now.Date.ToString("MM/dd"); // convert today's date to string for comparison to dates in recurringreminders
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");

            var rrDueToday = (context.RecurringReminders.Where(rr => rr.RepeatFrequencyName.RepeatFrequencyName == "Annually" &&
                                                               today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
                                                               (today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 || today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0) &&
                                                               (DateTime.Now.Date.ToString("MM/dd").CompareTo(rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0 || rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo("2001") == 0)).ToList());


            Console.WriteLine("We are after the var statement");
            Console.WriteLine("Count of Annual Reminders in var rrDueToday: " + rrDueToday.Count());
            

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
            foreach (var rr in rrDueToday)
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

                    rr.RecurringReminderDateAndTimeLastAlertSent = DateTime.Now;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }

            }

            //

            return null;
        }

        public IActionResult SendRecurringReminderTextsOnce()

        {
            // create a list of the Once reminders that are scheduled to go out today

            string today = DateTime.Now.Date.ToString("MM/dd/yyyy"); // convert today's date to string for comparison to dates in recurringreminders
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");

            var rrDueToday = (context.RecurringReminders.Where(rr => rr.RepeatFrequencyName.RepeatFrequencyName == "Once" &&
                                                               today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd/yyyy")) >= 0 && 
                                                               today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd/yyyy")) <= 0 &&
                                                               (DateTime.Now.Date.ToString("MM/dd").CompareTo(rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0 || rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo("2001") == 0)).ToList());


            Console.WriteLine("We are after the var statement");
            Console.WriteLine("Count of Once Reminders in var rrDueToday: " + rrDueToday.Count());


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
            foreach (var rr in rrDueToday)
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

                    rr.RecurringReminderDateAndTimeLastAlertSent = DateTime.Now;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
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
