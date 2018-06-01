
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class NonRecurringReminders
    {
        public int ID { get; set; }
        public string NonRecurringReminderName { get; set; }
        public string NonRecurringReminderDescription { get; set; }
        public string NonRecuringReminderCreateDate { get; set; }
        public DateTime NonRecurringEventDate { get; set; }
        public string NonRecurringEventTime { get; set; }
        public DateTime NonRecurringReminderStartAlertDate { get; set; }
        public DateTime NonRecurringReminderLastAlertDate { get; set; }
        public string NonRecurringReminderFirstAlertTime { get; set; }
        public string NonRecurringReminderSecondAlertTime { get; set; }
        public string NonRecuringReminderAlertFrequency { get; set; }  //how often the alert is sent for an event
        public string UserCellPhoneNumber { get; set; }
        public DateTime NonRecurringReminderDateAndTimeLastAlertSent { get; set; }  //initial value to be set to null hence the ?

        public int UserId { get; set; }
        public User User { get; set; }



        //default constructor
        public NonRecurringReminders()
        {
            // NonRecurringReminderId = nextNonRecurringReminderId;
            // nextNonRecurringReminderId++;
            NonRecuringReminderCreateDate = DateTime.Today.ToString("MM/dd/yyyy");
            NonRecurringReminderDateAndTimeLastAlertSent = new DateTime(2002, 08, 10); //set a date in the past

        }
        //non default constructor

        public NonRecurringReminders(string nonRecurringReminderName,
                                     string nonRecurringReminderDescription,
                                     DateTime nonRecurringEventDate,
                                     string nonRecurringEventTime,
                                     DateTime nonRecurringReminderStartAlertDate,
                                     DateTime nonRecurringReminderLastAlertDate,
                                     string nonRecurringReminderFirstAlertTime,
                                     string nonRecurringReminderSecondAlertTime,
                                     string nonRecuringReminderAlertFrequency) : this()
        {
            NonRecurringReminderName = nonRecurringReminderName;
            NonRecurringReminderDescription = nonRecurringReminderDescription;
            //NonRecurringReminderStartAlertDate = RemoveChar(nonRecurringReminderStartAlertDate, "/");
            NonRecurringReminderStartAlertDate = nonRecurringReminderStartAlertDate;
            //NonRecurringReminderLastAlertDate = RemoveChar(nonRecurringReminderLastAlertDate, "/");
            NonRecurringReminderLastAlertDate = nonRecurringReminderLastAlertDate;
            NonRecurringReminderFirstAlertTime = ConvertTo24HourFormat(nonRecurringReminderFirstAlertTime);
            NonRecurringReminderSecondAlertTime = ConvertTo24HourFormat(nonRecurringReminderSecondAlertTime);
            NonRecuringReminderAlertFrequency = nonRecuringReminderAlertFrequency;
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
