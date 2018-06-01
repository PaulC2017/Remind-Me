using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RemindMe.Models
{
    public class NonRecurringEvents
    {
        public int ID { get; set; }
        public string NonRecurringEventName { get; set; }
        public string NonRecurringEventDescription { get; set; }
        public string NonRecuringEventCreateDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        //default constructor
        public NonRecurringEvents()
        {
            //NonRecurringEventId = nextNonRecurringEventId;
            //nextNonRecurringEventId++;
            NonRecuringEventCreateDate = DateTime.Today.ToString("MM/dd/yyyy");

        }
        //non default constructor
        public NonRecurringEvents(string nonRecurringEventName, string nonRecurringEventDescription) : this()
        {
            NonRecurringEventName = nonRecurringEventName;
            NonRecurringEventDescription = nonRecurringEventDescription;


        }


    }
}

