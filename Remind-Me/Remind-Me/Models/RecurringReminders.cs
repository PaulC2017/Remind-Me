
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindMe.Models;

namespace RemindMe.Models
{
    public class RecurringReminders
    {
        public int ID { get; set; }
        public string RecurringReminderName { get; set; }  // this is also the event name
        public string RecurringReminderDescription { get; set; } // this is also the event description
        public DateTime RecurringEventDate { get; set; }
        public string RecuringReminderCreateDate { get; set; }
        
        
        public DateTime RecurringReminderStartAlertDate { get; set; } 
                  
        public DateTime RecurringReminderLastAlertDate { get; set; }
        public string RecurringReminderFirstAlertTime { get; set; }
        public string RecurringReminderSecondAlertTime { get; set; }
        // does the event repeat annually, monthly, Once, etc?

        public int RepeatFrequencyNameID { get; set; }
        public ReminderRepeatFrequencies RepeatFrequencyName { get; set; }

        //

        
        public string UserCellPhoneNumber { get; set; }
        public DateTime RecurringReminderDateAndTimeLastAlertSent { get; set; }  


        public int UserId { get; set; }
        public User User { get; set; }

        /*
        public int SendRemindersMidnightToFiveAmId { get; set; }
        public SendRemindersMidnightToFiveAm SendRemindersMidnightToFiveAm { get; set; }
        */

        //default constructor
        public RecurringReminders()
        {
            //RecurringReminderId = nextRecurringReminderId;
            //nextRecurringReminderId++;
            RecuringReminderCreateDate = DateTime.Today.ToString("MM/dd/yyyy");
            RecurringReminderFirstAlertTime = "0900";
            RecurringReminderSecondAlertTime = "1500";
            
            RecurringReminderDateAndTimeLastAlertSent = new DateTime(2001, 01, 01); // year, month, day - set a date that will always be less than the current date
        }
        //non default constructor
        public RecurringReminders(string recurringReminderName,
                                  string recurringReminderDescription,
                                  DateTime recurringEventDate,
                                  DateTime recurringReminderStartAlertDate,
                                  DateTime recurringReminderLastAlertDate,
                                  ReminderRepeatFrequencies recurringReminderRepeatFrequency,
                                  string reminderTime,
                                  string reminderTime2,
                                  string userCellPhoneNumber) : this()
        {
            RecurringReminderName = recurringReminderName;
            RecurringReminderDescription = recurringReminderDescription;
            RecurringEventDate = recurringEventDate;
            RecurringReminderStartAlertDate = recurringReminderStartAlertDate;
            RecurringReminderLastAlertDate = recurringReminderLastAlertDate;
            RepeatFrequencyName = recurringReminderRepeatFrequency;
            RecurringReminderFirstAlertTime = reminderTime;
            RecurringReminderSecondAlertTime = reminderTime2;
            UserCellPhoneNumber = userCellPhoneNumber;


        }
        
    }
}
