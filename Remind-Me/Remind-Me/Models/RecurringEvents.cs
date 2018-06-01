using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class RecurringEvents
    {
        public int ID { get; set; }
        public string RecurringEventName { get; set; }
        public string RecurringEventDescription { get; set; }
        public DateTime RecurringEventDate { get; set; }
        public string RecuringEventCreateDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        //default constructor
        public RecurringEvents()
        {
            //RecurringEventId = nextRecurringEventId;
            //nextRecurringEventId++;
            RecuringEventCreateDate = DateTime.Today.ToString("MM/dd/yyyy");

        }
        //non default constructor
        public RecurringEvents(string recurringEventName, string recurringEventDescription, DateTime recurringEventDate) : this()
        {
            RecurringEventName = recurringEventName;
            RecurringEventDescription = recurringEventDescription;
            RecurringEventDate = recurringEventDate;

        }

    }
}
