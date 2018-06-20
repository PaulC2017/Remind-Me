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
    public class AddTextCredentialsAndFrequenciesViewModel
    {
        
        [Display(Name = "Frequency One (Optional)")]
        public string RepeatFrequencyNameOne { get; set; }

        
        [Display(Name = "Frequency Two (Optional)")]
        public string RepeatFrequencyNameTwo { get; set; }

        
        [Display(Name = "Text User Id (Optional)")]
        public string TextUserId { get; set; }

        
        [Display(Name = "Text Token (Optional)")]
        public string TextToken { get; set; }

        
        [Display(Name = "Text Secret (Optional)")]
        public string TextSecret { get; set; }

        
        [Display(Name = "Text From (Optional)")]
        public string TextFrom { get; set; }

        public AddTextCredentialsAndFrequenciesViewModel() //default constructor needed 
        {                                                  //to make model binding work

        }

    }
}
