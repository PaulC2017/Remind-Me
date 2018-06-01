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
            // launch annual recurringreminders at 10:00 am every day


            //RecurringJob.AddOrUpdate("Annual_Reminders", () => SendRecurringReminderTextsAnnually(), "44 10 * * *");  // every day at 10:44 am
            //RecurringJob.AddOrUpdate("Annual_Reminders", () => SendRecurringReminderTextsAnnually()).Daily);




            //
            Console.WriteLine("We are before the SendReminderTextAnually Statement");
            BackgroundJob.Enqueue(() => SendRecurringReminderTextsAnnually());
            Console.WriteLine("We are after the SendReminderTextAnually Statement");

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


            //checkUserName = context.User.Single(u => u.Username == registerUserViewModel.Username);

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
            ScheduleEventsAndRemindersViewModel scheduleEventsAndReminder = new ScheduleEventsAndRemindersViewModel(context.User.ToList());

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

                User newUser = context.User.Single(u => u.Username == HttpContext.Session.GetString("Username"));
                // string userCellPhoneNumber = newUser.CellPhoneNumber;
                RecurringReminders newRecurringReminder = new
                                 RecurringReminders(newEventAndReminder.RecurringEventName,
                                 newEventAndReminder.RecurringEventDescription,
                                 newEventAndReminder.RecurringEventDate,
                                 newEventAndReminder.RecurringReminderStartAlertDate,
                                 newEventAndReminder.RecurringReminderLastAlertDate,
                                 newEventAndReminder.RecurringReminderRepeatFrequency,
                                 newEventAndReminder.UserCellPhoneNumber);

                newRecurringReminder.User = newUser;


                context.RecurringReminders.Add(newRecurringReminder);

                // save the new event and reminder to the data base

                context.SaveChanges();
                ViewBag.eventDate = newEventAndReminder.RecurringEventDate;
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
                                                  ch.RecurringReminderRepeatFrequency,
                                                  ch.UserCellPhoneNumber
                                              }).AsEnumerable().Select(c => c.ToExpando());

            /*
            var userRecurringReminders = (from recurringReminder in context.RecurringReminders
                                          where (recurringReminder.UserId == HttpContext.Session.GetInt32("ID"))
                                          select new
                                          {
                                              recurringReminder.RecurringReminderName,
                                              recurringReminder.RecurringReminderDescription,
                                              recurringReminder.RecurringEventDate,
                                              recurringReminder.RecurringReminderStartAlertDate,
                                              recurringReminder.RecurringReminderLastAlertDate,
                                              recurringReminder.RecurringReminderRepeatFrequency
                                          }).ToList();
             */

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

        // Method called from Startup.cs that launches Hangfire background tasks

        public IActionResult LaunchBackGroundJobs(Object j)
        {
            //SendReminderTextsController texts = new SendReminderTextsController();

            //BackgroundJob.Enqueue(() => texts.SendAnnualTexts());

            BackgroundJob.Enqueue(() => SendRecurringReminderTextsAnnually());

            return null;
        }

        public IActionResult SendRecurringReminderTextsAnnually()

        {
            // create a list of the reminders that are scheduled to go out today
            DateTime today = DateTime.Now.Date;
            Console.WriteLine("today = " + today);
            Console.WriteLine("We are before the var statement");
            var rrDueToday = (context.RecurringReminders.Where(rr => rr.RecurringReminderRepeatFrequency == "Annually" &&
                                                                  today >= rr.RecurringReminderStartAlertDate.Date &&
                                                                  today <= rr.RecurringReminderLastAlertDate.Date).ToList());
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

                    string eventDate = rr.RecurringEventDate.ToString("MM/dd");
                    string textMessage = rr.RecurringReminderName + "\r\n" + eventDate;

                    SendMessage(rr.UserCellPhoneNumber, TextFrom, textMessage, TextId, TextToken, TextSecret).Wait();
                    Console.WriteLine("We are after the TRY command");

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

    }

}