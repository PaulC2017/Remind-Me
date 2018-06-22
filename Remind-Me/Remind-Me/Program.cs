using System;


using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RemindMe.Data;

namespace RemindMe
{
    public class Program
    {

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseSetting("detailedErrors", "true")
            .CaptureStartupErrors(true)
            .Build();


    }
}
