
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string RecurringReminderRepeatFrequency { get; set; }  // does the event repeat annually, monthly, etc?
        public string UserCellPhoneNumber { get; set; }
        public DateTime RecurringReminderDateAndTimeLastAlertSent { get; set; }  //initial value to be set to null hence the ?


        public int UserId { get; set; }
        public User User { get; set; }

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
                                  string recurringReminderRepeatFrequency,
                                  string userCellPhoneNumber) : this()
        {
            RecurringReminderName = recurringReminderName;
            RecurringReminderDescription = recurringReminderDescription;
            RecurringEventDate = recurringEventDate;
            RecurringReminderStartAlertDate = recurringReminderStartAlertDate;
            RecurringReminderLastAlertDate = recurringReminderLastAlertDate;
            RecurringReminderRepeatFrequency = recurringReminderRepeatFrequency;
            UserCellPhoneNumber = userCellPhoneNumber;


        }
        public string RemoveChar(string theString, string charsToRemove)
        {
            theString = theString.Replace(charsToRemove, "");
            return theString;
        }

        public string ConvertTo24HourFormat(string time)
        {
            time = RemoveChar(time, ":");
            if (time.IndexOf("AM") == -1)
            {
                time = RemoveChar(time, "PM");
                int timeInt = int.Parse(time);
                timeInt = timeInt + 12;
                time = timeInt.ToString();
            }
            else
            {
                time = RemoveChar(time, "AM");
            }
            return time;
        }
    }
}
