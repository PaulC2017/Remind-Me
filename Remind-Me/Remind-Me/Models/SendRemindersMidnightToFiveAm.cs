using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class SendRemindersMidnightToFiveAm
    {
        public int ID { get; set; }

        public string TimeToSendReminder { get; set; }

        public int RecurringReminderId { get; set; }
        public RecurringReminders RecurringReminder { get; set; }

        public IList<RecurringReminders> RecurringReminders { get; set; }
    }
}
