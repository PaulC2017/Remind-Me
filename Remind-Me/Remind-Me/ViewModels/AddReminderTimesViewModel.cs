using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.ViewModels
{
    public class AddReminderTimesViewModel
    {

        [Display(Name = "Reminder Time ")]
        public string ReminderTimesName { get; set; }

        public AddReminderTimesViewModel() //default constructor needed 
        {                                  //to make model binding work

        }
    }
}
