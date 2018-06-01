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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, IApplicationLifetime lifetime,
                              IRecurringJobManager recurringJobs)
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



            //added for Hangfire

            GlobalConfiguration.Configuration.UseSqlServerStorage((Configuration.GetConnectionString("DefaultConnection")));
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //start and configure recurring Hangfire jobs on startup

            app.UseMvc();
            recurringJobs.AddOrUpdate("Annual_Reminders", Job.FromExpression<RemindMeController>(x => x.LaunchBackGroundJobs(null)), Cron.Daily(19, 35)); //UTC time of 4 hours ahead of Eastern Daylight Time (EDT)
            //RecurringJob.AddOrUpdate("Annual_Reminders", () => SendRecurringReminderTextsAnnually(), "44 10 * * *");  // every day at 10:44 am


            // this one works!!
            //
            // recurringJobs.AddOrUpdate("Annual_Reminders", Job.FromExpression<RemindMeController>(x => x.LaunchBackGroundJobs(null)), Cron.Minutely());
            //

        }


    }
}
