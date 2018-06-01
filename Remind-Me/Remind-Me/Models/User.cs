using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindMe.ViewModels;
using RemindMe.Models;
using RemindMe.Data;
using Microsoft.EntityFrameworkCore;

namespace RemindMe.Models
{
    public class User
    {


        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string GCalEmail { get; set; }
        public string GCalEmailPassword { set; get; }
        public string CellPhoneNumber { get; set; }
        public string UserCreateDate { get; set; }


        public IList<RecurringEvents> RecurringEvents { get; set; }
        public IList<NonRecurringEvents> NonRecurringEvents { get; set; }
        public IList<RecurringReminders> RecurringReminders { get; set; }
        public IList<NonRecurringReminders> NonRecurringReminders { get; set; }



        //default constructor
        public User()
        {
            //UserId = nextUserId;
            //nextUserId++;
            UserCreateDate = DateTime.Today.ToString("MM/dd/yyyy");

        }

        //non default constructor
        public User(string username, string password, string gCalEmail, string gCalEmailPassword, string cellPhoneNumber) : this()
        {
            Username = username;
            Password = password;
            GCalEmail = gCalEmail;
            GCalEmailPassword = gCalEmailPassword;
            CellPhoneNumber = cellPhoneNumber;

        }




    }
}
