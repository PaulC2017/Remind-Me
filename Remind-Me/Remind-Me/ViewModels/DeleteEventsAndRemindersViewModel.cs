using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindMe.Data;
using RemindMe.Models;
using Microsoft.EntityFrameworkCore;

namespace RemindMe.ViewModels
{
    public class DeleteEventsAndRemindersViewModel
    {
        public int recurringReminderId { get; set; }

        [Required(ErrorMessage = "An Event Name is Required")]
        [Display(Name = "Event Name")]
        public string RecurringEventName { get; set; }

        [Required(ErrorMessage = "You Must Enter Yes or No")]
        [Display(Name = "Are you sure you want to delete this Reminder?")]
        public string DeleteReminder { get; set; }

        //default constructor needed to make model binding work

        public DeleteEventsAndRemindersViewModel()
        {

        }
        
    }
}
