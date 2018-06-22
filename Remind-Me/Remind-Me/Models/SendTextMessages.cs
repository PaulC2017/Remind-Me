using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindMe.ViewModels;
using Bandwidth.Net;
using Bandwidth.Net.Api;
using RemindMe.Models;
using RemindMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.ObjectModel;
using System.Collections;
using System.Dynamic;
using Hangfire;
using System.Data;
using RemindMe.Controllers;

namespace RemindMe.Models
{
    public class SendTextMessages
    {
        private readonly RemindMeDbContext context;

        public SendTextMessages(RemindMeDbContext dbContext)
        {
            context = dbContext;
        }
        public IActionResult SendConfirmation(string cellPhoneNumber, string eventName, DateTime eventDate, string description, DateTime startReminders, DateTime stopReminders)

        {
        
            //Get TextInfo and populate text parameters//
            string TextId = "";
            string TextToken = "";
            string TextSecret = "";
            string TextFrom = "";

            var textInfo = (context.TextInfo.Where(id => id.ID > 0).ToList()); // there is only 1 record in this table

            foreach (var ti in textInfo)
            {
                TextId = ti.TextUserId;
                TextToken = ti.TextToken;
                TextSecret = ti.TextSecret;
                TextFrom = ti.TextFrom;
            }
            
            
            // send text message
            
                
                    
           // format text message
           string sEventDate = eventDate.ToString("MM/dd"); // Just include the month and day of the event in the text message
           string textMessage = "From: TheReminderFactory" + "\u2122" + "\r\n" +
                "Event: " + eventName + "\r\n" + "Description: " +
                description + "\r\nEvent Date: " + eventDate +
                " Reminders will start " + startReminders.ToString("MM/dd") +
                " Reminders will end " + stopReminders.ToString("MM/dd"); 
            
            //RemindMeController.SendMessage(cellPhoneNumber, TextFrom, textMessage, TextId, TextToken, TextSecret).Wait();
            
            return null;
            
        }

    }
}
