using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindMe.Data;
using RemindMe.Models;
using Microsoft.EntityFrameworkCore;

namespace RemindMe.ViewModels
{
    public class EditScheduleEventsAndRemindersViewModel
    {

        public int recurringReminderId { get; set; }

        [Required(ErrorMessage = "An Event Name is Required")]
        [Display(Name = "Event Name")]
        public string RecurringEventName { get; set; }

        // we will use this value for the RecurringReminderName in the controller

        
        [Display(Name = "Event Description (Optional)")]
        public string RecurringEventDescription { get; set; }

        // we will use the above  value for the RecurringReminderDescription in the controller

        [Required(ErrorMessage = "You must enter a date")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd}")]
        public DateTime RecurringEventDate { get; set; }


        [Required]
        [Display(Name = "RepeatFrequencyName")]
        public int RepeatFrequencyNameID { get; set; }
        public List<SelectListItem> Frequencies { get; set; }

        [Required(ErrorMessage = "Enter the date you want to start receiving the Reminders")]
        [DataType(DataType.Date)]
        [Display(Name = "Date to Start Sending Reminders")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd}")]
        public DateTime RecurringReminderStartAlertDate { get; set; }

        [Required]
        [Display(Name = "ReminderTimes")]
        private int reminderTimesID = 10; //setting defaut value in drop down list
        public int ReminderTimesID { get { return reminderTimesID; } set { reminderTimesID = value; } }// set default value to ID = 10 in List<SelectListItem> - 9:00 Am
        public List<SelectListItem> ReminderTimes { get; set; }

        [Display(Name = "ReminderTimes2")]
        private int reminderTimes2ID = 0;  // setting default value in drop down list
        public int ReminderTimes2ID { get { return reminderTimes2ID; } set { reminderTimes2ID = value; } }  //set default value to ID = 0 (9:00 AM)
        public List<SelectListItem> ReminderTimes2 { get; set; }

        [Required(ErrorMessage = "Enter the date to stop receiving the Reminders")]
        [DataType(DataType.Date)]
        [Display(Name = "Date to Stop Sending Reminders")]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd}")]
        public DateTime RecurringReminderLastAlertDate { get; set; }

        [Required(ErrorMessage = "Enter the cell phone number where you want to receive the text reminders (ie - 2125551212)")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Cell Phone Number (Enter Numbers Only.  ie -  2125551212) ")]
        public string UserCellPhoneNumber { get; set; }


        // we will send a text message once a day on from start date through the last alart date


        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        public List<SelectListItem> Users { get; set; }
        public string currentUser;







        // set up drop down box for reminder frequency for user to select




        //

        //default constructor needed to make model binding work
        public EditScheduleEventsAndRemindersViewModel() 
        {                                            

        }
        public EditScheduleEventsAndRemindersViewModel(IEnumerable<ReminderRepeatFrequencies> repeatFrequencies,
            IEnumerable<ReminderTimes> reminderTimes)  // default constructor  
        {
            //Code for drop down box for reminder frequency for user to select

            Frequencies = new List<SelectListItem>();

            foreach (ReminderRepeatFrequencies rrf in repeatFrequencies.ToList())

            {


                Frequencies.Add(new SelectListItem
                {
                    Value = (rrf.ID.ToString()),
                    Text = rrf.RepeatFrequencyName.ToString()

                });

            }

            //Code for drop down box for reminder times for user to select - 1st reminder of the day

            ReminderTimes = new List<SelectListItem>();
            foreach (ReminderTimes rt in reminderTimes)
            {
                ReminderTimes.Add(new SelectListItem
                {
                    Value = (rt.ID.ToString()),
                    Text = rt.ReminderTimesName.ToString(),
                    //Selected = rt.ID == 10 ? true : false

                });
            }


            //Code for drop down box for reminder times for user to select - 2nd reminder of the day


            ReminderTimes2 = new List<SelectListItem>();

            //add "Do Not Select" option
            SelectListItem doNotSelect = new SelectListItem() { Text = "Do Not Schedule", Value = "0" };
            ReminderTimes2.Add(doNotSelect);

            foreach (ReminderTimes rt in reminderTimes)
            {

                ReminderTimes2.Add(new SelectListItem
                {
                    Value = (rt.ID.ToString()),
                    Text = rt.ReminderTimesName.ToString()

                });
            }

        }

        public EditScheduleEventsAndRemindersViewModel(IEnumerable<User> users)
        {

            Users = new List<SelectListItem>();

            foreach (User user in users.ToList())

            {

                Users.Add(new SelectListItem
                {
                    Value = (user.ID.ToString()),
                    Text = user.Username.ToString()

                });

            }

        }
    }
}
