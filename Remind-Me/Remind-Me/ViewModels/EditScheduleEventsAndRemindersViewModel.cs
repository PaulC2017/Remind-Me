using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.ViewModels
{
    public class EditScheduleEventsAndRemindersViewModel : ScheduleEventsAndRemindersViewModel
    {
        public int RecurringReminderId { get; set; }
    }
}
