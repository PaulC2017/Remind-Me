using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class SendRemindersNoonToFivePm
    {
        public int ID { get; set; }

        public string TimeToSendReminderNTFPM { get; set; }
        public int RecurringReminderId { get; set; }
        public int SendRemindersTimeFrameID { get; } = 2;
        public string FirstOrSecondReminderOfTheDay { get; set; }
    }
}

