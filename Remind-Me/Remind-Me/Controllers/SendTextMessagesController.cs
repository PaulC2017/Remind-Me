using Bandwidth.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RemindMe.Controllers
{
    public class SendTextMessagesController : Controller
    {
        
        //default constructor
        public SendTextMessagesController()
        {
        }
        // non default constructor
        public  IActionResult SendTextMessage(string cellPhoneNumber, 
            string eventName, DateTime eventDate, string description, 
            DateTime startReminders, DateTime stopReminders, 
            List<RemindMe.Models.TextInfo> textInfo,
            string repeatFreqNameUserSelected)

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
                     "Confirmation of Event Reminder schedule" +
                     "\r\nEvent: " + eventName + "\r\n" + 
                     "Description: " +description + 
                     "\r\nEvent Date: " + sEventDate +
                     "\r\nReminders will start: " + startReminders.ToString("MM/dd") +
                     "\r\nReminders will end: " + stopReminders.ToString("MM/dd") +
                     "\r\nThis schedule of Reminders will be sent: " + repeatFreqNameUserSelected;

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


