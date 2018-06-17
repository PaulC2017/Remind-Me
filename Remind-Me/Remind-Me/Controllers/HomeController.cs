using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remind_Me.Models;

namespace Remind_Me.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("../RemindMe/Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Using TheReminderFactory";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "TheReminderFactory Support";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
