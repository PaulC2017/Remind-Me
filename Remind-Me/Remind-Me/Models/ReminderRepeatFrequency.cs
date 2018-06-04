using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class ReminderRepeatFrequency
    {
        public string RepeatFrequencyName { get; set; }
        public int ID { get; set; }

        public IList<RecurringReminders> Reminders { get; set; }
        
    }
}
