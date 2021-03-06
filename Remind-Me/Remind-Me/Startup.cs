﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemindMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Google;
using RemindMe.Controllers;
using Hangfire.Common;
using RemindMe.Models;
using Hangfire.Annotations;
using Microsoft.AspNetCore.Owin;
using Remind_Me.Services;
using Remind_Me.Models;

namespace RemindMe
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Hangfire - comment this out when migrating and updating the database
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            //



            services.AddMvc();

            // Access to database

            services.AddDbContext<RemindMeDbContext>(options =>
                                                     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //

            //Sessions
            services.AddMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".RemindMe";
            });
            //


            /*
            // Google Calendar API

            services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddAuthentication(
                                       v =>
                                       {
                                           v.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                                           v.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

                                       }).AddGoogle(googelOptions =>
                                       {
                                           googelOptions.ClientId = "723770158612-3h09hjsoaei13hm6k3n4dp3dip402r5a.apps.googleusercontent.com";
                                           googelOptions.ClientSecret = "8_lrbaCJxOjUmCrLPCYD5VSJ";
                                       });
            //


            */

            //LogIn/Authenticate using Google + ID

            services.AddIdentity<ApplicationUser, IdentityRole>() //note User is the Model where User IDs are stored
                .AddEntityFrameworkStores<RemindMeDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "723770158612 - 425ap2p15st6gplnkffitgmccu1j1ca4.apps.googleusercontent.com";
                googleOptions.ClientSecret = "zicbsx_Anbf0GLduxcHKMLjh";

                //googleOptions.ClientId = Configuration["723770158612 - 425ap2p15st6gplnkffitgmccu1j1ca4.apps.googleusercontent.com"];
                //googleOptions.ClientSecret = Configuration["zicbsx_Anbf0GLduxcHKMLjh"];
            });

            // Add application services.   added as part of Google ID sign on feature
            // not sure if this is really needed for that feature
            services.AddTransient<IEmailSender, EmailSender>();

            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        // Use this one to include hangfire after migrating and updating the database

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, IApplicationLifetime lifetime,
                              IRecurringJobManager recurringJobs)


        // use this one when migrating and updating the database - it removes IRecurringJobManager recurringJobs
        /*
            public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                                  ILoggerFactory loggerFactory, IApplicationLifetime lifetime)

                */

        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //  google calendar

            //app.UseAuthentication().UseMvc();  removed temporarily while I decide what to do with the Google calendar

            //





            // Sessions
            app.UseSession();

            //


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");
            });



            //added for Hangfire - comment thenm out when migrating and updating the database

            GlobalConfiguration.Configuration.UseSqlServerStorage((Configuration.GetConnectionString("DefaultConnection")));
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //

            //start and configure recurring Hangfire jobs on startup

            app.UseMvc();



            //added for Hangfire - comment it out when migrating and updating the database

            // Send reminders scheduled on the hour
            recurringJobs.AddOrUpdate("Send_Reminders_Hour", Job.FromExpression<RemindMeController>
                                      (x => x.LaunchSendRecurringReminderTexts(null)), Cron.Hourly()); //UTC (HR, Min) time of 4 hours ahead of EDT and 5 hours ahead of EST

            //send reminders scxheduled on the half hour
            recurringJobs.AddOrUpdate("Send_Reminders_Half_Hour", Job.FromExpression<RemindMeController>
                                      (x => x.LaunchSendRecurringReminderTexts(null)), Cron.Hourly(30)); //UTC (HR, Min) time of 4 hours ahead of EDT and 5 hours ahead of EST

            //reference for how to schedule using cron expressions:  RecurringJob.AddOrUpdate("Annual_Reminders", () => SendRecurringReminderTextsAnnually(), "44 10 * * *");  // every day at 10:44 am UTC Time

            //added for Hangfire - comment it out when migrating and updating the database

            // launch annual reset of RecurringReminderDateAndTimeLastAlertSent
            // we have to set the dates to 01/01 so the logic in the SendRecurringReminderTextsAnnually() method will work correctly in a new year

            recurringJobs.AddOrUpdate("Reset_RecurringReminderDateAndTimeLastAlertSent",
                                      Job.FromExpression<RemindMeController>(x => x.LaunchResetRecurringReminderDateAndTimeLastAlertSent(null)),
                                      Cron.Yearly(01, 01, 04, 30)); //()Month,day,Hour, minute  in UTC - starts at the first minute of the hour 
                                                                    // note UTC is +5 hours to EST and +4 in EDT

            //

            // this one works!!
            //
            // recurringJobs.AddOrUpdate("Annual_Reminders", Job.FromExpression<RemindMeController>(x => x.LaunchBackGroundJobs(null)), Cron.Minutely());
            //

        }


    }
}
