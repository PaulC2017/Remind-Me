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


namespace RemindMe.Controllers
{
    public class SendTextMessagesController : Controller
    {
        
        //default constructor
        public SendTextMessagesController()
        {
        }
        // non default constructor
        public  IActionResult SendTextMessage(string cellPhoneNumber, string eventName, DateTime eventDate, string description, DateTime startReminders, DateTime stopReminders, List<RemindMe.Models.TextInfo> textInfo)

            {

                //Get TextInfo and populate text parameters//
                string TextId = "";
                string TextToken = "";
                string TextSecret = "";
                string TextFrom = "";
            
                foreach (var ti in textInfo) // only 1 record in this list
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
                     description + "\r\nEvent Date: " + eventDate.ToString("DD/mm/yyyy") +
                     " Reminders will start " + startReminders.ToString("MM/dd") +
                     " Reminders will end " + stopReminders.ToString("MM/dd");

                SendMessage(cellPhoneNumber, TextFrom, textMessage, TextId, TextToken, TextSecret).Wait();

                return null;

        }
        private static async Task SendMessage(string to, string from, string text, string textId, string textToken, string textSecret)

        {
            var data = new Bandwidth.Net.Api.MessageData
            {
                To = to, // number receiving text essage
                From = from, //bandwidth number
                Text = text  // reminder
            };

            Console.WriteLine("Text ID = See PW Doc"); //+ textId;
            Console.WriteLine("Text Token = See PW Doc"); // + textToken);
            Console.WriteLine("Text Secret = See PW Doc"); // + textSecret);
            Console.WriteLine("Text From = See PW Doc"); // + from);


            Console.WriteLine("We are before the var client command");
            var client = new Client(textId, textToken, textSecret);
            Console.WriteLine("We are after the var client command");

            Console.WriteLine("We are before the var message command");
            var message = await client.Message.SendAsync(data);
            Console.WriteLine("We are after the var message command");

        }
    }
}


