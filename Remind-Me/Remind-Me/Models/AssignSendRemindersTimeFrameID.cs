using RemindMe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RemindMe.Models
{
    //determine what times the user has selected for the reminder and 
    //save the data to the correcponding SendRemindersXXXXXXXX Table

   
    public class AssignSendRemindersTimeFrameID
    {
        string SendTimeReminderTimeSelected { get; set; }
        int TimeSelectedAsinteger = -99;

        public int DetermineSendReminderTime(string timeSelected)
        {
            // determine the send reminder time the user selected and return the 
            // integer corresponding to the group the time selected falls into
            // 0 - midnight to 5 am
            // 1 - 6 am to 11 am
            // 2 - noon to 5 pm
            // 3 - 6 pm to 11 pm

            SendTimeReminderTimeSelected = timeSelected;
           
            switch (SendTimeReminderTimeSelected)
            {
                case "12:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;

                case "01:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;

                case "02:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;

                case "03:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;

                case "04:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;

                case "05:00 AM":
                    TimeSelectedAsinteger = 0;
                    return TimeSelectedAsinteger;
                    
                case "06:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "07:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "08:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "09:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "10:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "11:00 AM":
                    TimeSelectedAsinteger = 1;
                    return TimeSelectedAsinteger;

                case "12:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "01:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "02:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "03:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "04:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "05:00 PM":
                    TimeSelectedAsinteger = 2;
                    return TimeSelectedAsinteger;

                case "06:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;

                case "07:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;

                case "08:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;

                case "09:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;

                case "10:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;

                case "11:00 PM":
                    TimeSelectedAsinteger = 3;
                    return TimeSelectedAsinteger;
                
                default:
                    TimeSelectedAsinteger = -99; //the user selected Do Not Schedule" from the drop down box
                    return TimeSelectedAsinteger;


            }

             
        }

    }
}
    
