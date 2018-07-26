using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class SendRemindersSixAmToElevenAm
    {
        public int ID { get; set; }

        public string TimeToSendReminderSTEAM { get; set; }
        public int RecurringReminderId { get; set; }
        public int SendRemindersTimeFrameID { get; } = 1;
        public string FirstOrSecondReminderOfTheDay { get; set; }

    }
}
