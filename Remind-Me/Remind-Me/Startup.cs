using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemindMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using RemindMe.Controllers;
using Hangfire.Common;
using RemindMe.Models;
using Hangfire.Annotations;

using Microsoft.AspNetCore.Owin;
//[assembly: OwinStartupAttribute(typeof(HangFire.Startup))]

namespace RemindMe
{
    public class Startup
    {
        /*
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        */
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
            //Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            //


            services.AddMvc();

            // Access to database

            services.AddDbContext<RemindMeDbContext>(options =>
                                                     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));




            //Sessions
            services.AddMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".RemindMe";
            });
            //

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

            // launch Annual Reminders Background Task

            recurringJobs.AddOrUpdate("Annual_Reminders", Job.FromExpression<RemindMeController>
                                      (x => x.LaunchSendRecurringReminderTextsAnnually(null)), Cron.Daily(19, 25)); //UTC (HR, Min) time of 4 hours ahead of EDT and 5 hours ahead of EST

            //
            // launch "Once" reminders Background task

            recurringJobs.AddOrUpdate("Once_Reminders", Job.FromExpression<RemindMeController>
                                      (x => x.LaunchSendRecurringReminderTextsOnce(null)), Cron.Daily(21, 35)); //UTC (HR, Min) time of 4 hours ahead of EDT and 5 hours ahead of EST

            //reference for how to schedule using cron expressions:  RecurringJob.AddOrUpdate("Annual_Reminders", () => SendRecurringReminderTextsAnnually(), "44 10 * * *");  // every day at 10:44 am

            //added for Hangfire - comment it out when migrating and updating the database

            // launch annual reset of RecurringReminderDateAndTimeLastAlertSent
            // we have to set the dates to 01/01 so the logic in the SendRecurringReminderTextsAnnually() method will work correctly in a new year

            recurringJobs.AddOrUpdate("Reset_RecurringReminderDateAndTimeLastAlertSent",
                                      Job.FromExpression<RemindMeController>(x => x.LaunchResetRecurringReminderDateAndTimeLastAlertSent(null)),
                                      Cron.Yearly(06, 06, 12, 37)); //()Month,day,Hour, minute  in UTC - starts at the first minute of the hour 
                                                                    // note UTC is +5 hours to EST and +4 in EDT

            //

            // this one works!!
            //
            // recurringJobs.AddOrUpdate("Annual_Reminders", Job.FromExpression<RemindMeController>(x => x.LaunchBackGroundJobs(null)), Cron.Minutely());
            //

        }


    }
}
