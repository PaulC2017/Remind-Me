using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class SendRemindersSixPmToElevenPm
    {
        public int ID { get; set; }

        public string TimeToSendReminderSTEPM { get; set; }
        public int RecurringReminderId { get; set; }
        public int SendRemindersTimeFrameID { get; } = 3;
        public string FirstOrSecondReminderOfTheDay { get; set; }

    }
}
