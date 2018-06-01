using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindMe.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace RemindMe.Models
{
    public class HangfireJobs
    {

        // Method called from Startup.cs that launches Hangfire background tasks

        public IActionResult DoJob(Object j)
        {
            //SendReminderTextsController texts = new SendReminderTextsController();

            // BackgroundJob.Enqueue(() => texts.SendAnnualTexts());

            return null;
        }
    }
}
