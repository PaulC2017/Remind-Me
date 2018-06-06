

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
        private readonly RemindMeDbContext context;

        public RemindMeController(RemindMeDbContext dbContext)
        {
            context = dbContext;
        }


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
                User newUser = new User(registerUserViewModel.Username, registerUserViewModel.Password, registerUserViewModel.GCalEmail, registerUserViewModel.GCalEmailPassword, registerUserViewModel.CellPhoneNumber);
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
            catch (System.InvalidOperationException)
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

            //create the ViewModel with the list of repeat frequency options ))
            ScheduleEventsAndRemindersViewModel scheduleEventsAndReminder = 
                new ScheduleEventsAndRemindersViewModel(context.ReminderRepeatFrequencies.ToList());  

            if (HttpContext.Session.GetString("Username") == "")
            {
                return View("Index");
            }
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View(scheduleEventsAndReminder);
        }


        [HttpPost]
        public IActionResult ScheduleEventsAndReminders(ScheduleEventsAndRemindersViewModel newEventAndReminder)

        {

            if (ModelState.IsValid)
            {
                // create recurring reminder record

                //first get the Repeat Frequency Selected by the User
                ReminderRepeatFrequency newReminderRepeatFrequency = context.ReminderRepeatFrequencies.Single(c => c.ID == newEventAndReminder.RecurringReminderRepeatFrequencyID);
                
                User newUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));
                // string userCellPhoneNumber = newUser.CellPhoneNumber;
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

            return View(newEventAndReminder);
        }

        public ActionResult SingleUserRecurringEventsAndReminders()
        {

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
                                                  ch.UserCellPhoneNumber
                                              }).AsEnumerable().Select(c => c.ToExpando());

            return View(userRecurringReminders);

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

            User currentUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));
            return View(currentUser);
        }

        // Methods called from Startup.cs that launches Hangfire background tasks

        public IActionResult LaunchSendRecurringReminderTextsAnnually(Object j)
        {

            BackgroundJob.Enqueue(() => SendRecurringReminderTextsAnnually());

            return null;
        }

        public IActionResult LaunchResetRecurringReminderDateAndTimeLastAlertSent(Object j)
        {

            BackgroundJob.Enqueue(() => ResetRecurringReminderDateAndTimeLastAlertSent());

            return null;
        }

        //

        public IActionResult SendRecurringReminderTextsAnnually()

        {
            // create a list of the reminders that are scheduled to go out today
            
            string today = DateTime.Now.Date.ToString("MM/dd"); // convert today's date to string for comparison to dates in recurringreminders
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");

            var rrDueToday = (context.RecurringReminders.Where(rr => rr.RepeatFrequencyName.RepeatFrequencyName == "Annually" &&
                                                               today.CompareTo(rr.RecurringReminderStartAlertDate.Date.ToString("MM/dd")) >= 0 &&
                                                               today.CompareTo(rr.RecurringReminderLastAlertDate.Date.ToString("MM/dd")) <= 0 &&
                                                               (DateTime.Now.Date.ToString("MM/dd").CompareTo(rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("MM/dd")) > 0 || rr.RecurringReminderDateAndTimeLastAlertSent.Date.ToString("yyyy").CompareTo("2001") == 0)).ToList());


            Console.WriteLine("We are after the var statement");
            Console.WriteLine("Count of items in var rrDueToday: " + rrDueToday.Count());
            //

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
            Console.WriteLine("Text ID = " + TextId);
            Console.WriteLine("Text Token = " + TextToken);
            Console.WriteLine("Text Secret = " + TextSecret);
            Console.WriteLine("Text From = " + TextFrom);
            //
            // send reminders
            foreach (var rr in rrDueToday)
            {
                try
                {
                    Console.WriteLine("We are before the TRY command");
                    Console.WriteLine("Text ID = " + TextId);
                    Console.WriteLine("Text Token = " + TextToken);
                    Console.WriteLine("Text Secret = " + TextSecret);
                    Console.WriteLine("Text From = " + TextFrom);
                    Console.WriteLine("RecurringReminderDateAndTimeLastAlertSent.Date = " + rr.RecurringReminderDateAndTimeLastAlertSent.Date);
                    Console.WriteLine("DateTime.Now.Date = " + DateTime.Now.Date);
                    Console.WriteLine("Comparison of above two variables = " + (DateTime.Now > rr.RecurringReminderDateAndTimeLastAlertSent));

                    // format text message
                    string eventDate = rr.RecurringEventDate.ToString("MM/dd"); // Just include the month and day of the event in the text message
                    string textMessage = "From: Remind Me - Don't Forget!!\r\n" + "Event: " + rr.RecurringReminderName + "\r\n" + "Description: " + rr.RecurringReminderDescription + "\r\nDate " + eventDate;

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



        //


        private static async Task SendMessage(string to, string from, string text, string textId, string textToken, string textSecret)

        {
            var data = new Bandwidth.Net.Api.MessageData
            {
                To = to, // number receiving text essage
                From = from, //bandwidth number
                Text = text  // reminder
            };

            Console.WriteLine("Text ID = " + textId);
            Console.WriteLine("Text Token = " + textToken);
            Console.WriteLine("Text Secret = " + textSecret);
            Console.WriteLine("Text From = " + from);


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
            
            ReminderRepeatFrequency annually = repeatFreq[0];  //this is a list with one element
           
            //retireve the recurring reminder records that are "annual" reminders

            var annualRecurringReminders = context.RecurringReminders.Where(rr => rr.RepeatFrequencyNameID == annually.ID).ToList();
            
            Console.WriteLine("Finished retrieving the records");
            Console.WriteLine("Number of records found: " + annualRecurringReminders.Count());
            
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
