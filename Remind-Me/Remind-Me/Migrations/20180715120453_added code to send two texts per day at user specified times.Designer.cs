﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RemindMe.Data;

namespace RemindMe.Migrations
{
    [DbContext(typeof(RemindMeDbContext))]
    [Migration("20180715120453_added code to send two texts per day at user specified times")]
    partial class addedcodetosendtwotextsperdayatuserspecifiedtimes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RemindMe.Models.NonRecurringEvents", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NonRecuringEventCreateDate");

                    b.Property<string>("NonRecurringEventDescription");

                    b.Property<string>("NonRecurringEventName");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("NonRecurringEvents");
                });

            modelBuilder.Entity("RemindMe.Models.NonRecurringReminders", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NonRecuringReminderAlertFrequency");

                    b.Property<string>("NonRecuringReminderCreateDate");

                    b.Property<DateTime>("NonRecurringEventDate");

                    b.Property<string>("NonRecurringEventTime");

                    b.Property<DateTime>("NonRecurringReminderDateAndTimeLastAlertSent");

                    b.Property<string>("NonRecurringReminderDescription");

                    b.Property<string>("NonRecurringReminderFirstAlertTime");

                    b.Property<DateTime>("NonRecurringReminderLastAlertDate");

                    b.Property<string>("NonRecurringReminderName");

                    b.Property<string>("NonRecurringReminderSecondAlertTime");

                    b.Property<DateTime>("NonRecurringReminderStartAlertDate");

                    b.Property<string>("UserCellPhoneNumber");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("NonRecurringReminders");
                });

            modelBuilder.Entity("RemindMe.Models.RecurringEvents", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RecuringEventCreateDate");

                    b.Property<DateTime>("RecurringEventDate");

                    b.Property<string>("RecurringEventDescription");

                    b.Property<string>("RecurringEventName");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("RecurringEvents");
                });

            modelBuilder.Entity("RemindMe.Models.RecurringReminders", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RecuringReminderCreateDate");

                    b.Property<DateTime>("RecurringEventDate");

                    b.Property<DateTime>("RecurringReminderDateAndTimeLastAlertSent");

                    b.Property<string>("RecurringReminderDescription");

                    b.Property<string>("RecurringReminderFirstAlertTime");

                    b.Property<DateTime>("RecurringReminderLastAlertDate");

                    b.Property<string>("RecurringReminderName");

                    b.Property<string>("RecurringReminderSecondAlertTime");

                    b.Property<DateTime>("RecurringReminderStartAlertDate");

                    b.Property<int>("RepeatFrequencyNameID");

                    b.Property<int>("TimeToSendReminderDuplicatesID");

                    b.Property<int>("TimeToSendReminderMTFAMID");

                    b.Property<int>("TimeToSendReminderNTFPMID");

                    b.Property<int>("TimeToSendReminderSTEAMID");

                    b.Property<int>("TimeToSendReminderSTEPMID");

                    b.Property<string>("UserCellPhoneNumber");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("RepeatFrequencyNameID");

                    b.HasIndex("UserId");

                    b.ToTable("RecurringReminders");
                });

            modelBuilder.Entity("RemindMe.Models.ReminderRepeatFrequencies", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RepeatFrequencyName");

                    b.HasKey("ID");

                    b.ToTable("ReminderRepeatFrequencies");
                });

            modelBuilder.Entity("RemindMe.Models.ReminderTimes", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ReminderTimesName");

                    b.HasKey("ID");

                    b.ToTable("ReminderTimes");
                });

            modelBuilder.Entity("RemindMe.Models.SendRemindersMidnightToFiveAm", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RecurringReminderId");

                    b.Property<string>("TimeToSendReminderMTFAM");

                    b.HasKey("ID");

                    b.ToTable("SendRemindersMidnightToFiveAm");
                });

            modelBuilder.Entity("RemindMe.Models.SendRemindersNoonToFivePm", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RecurringReminderId");

                    b.Property<string>("TimeToSendReminderNTFPM");

                    b.HasKey("ID");

                    b.ToTable("SendRemindersNoonToFivePm");
                });

            modelBuilder.Entity("RemindMe.Models.SendRemindersSixAmToElevenAm", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RecurringReminderId");

                    b.Property<string>("TimeToSendReminderSTEAM");

                    b.HasKey("ID");

                    b.ToTable("SendRemindersSixAmToElevenAm");
                });

            modelBuilder.Entity("RemindMe.Models.SendRemindersSixPmToElevenPm", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RecurringReminderId");

                    b.Property<string>("TimeToSendReminderSTEPM");

                    b.HasKey("ID");

                    b.ToTable("SendRemindersSixPmToElevenPm");
                });

            modelBuilder.Entity("RemindMe.Models.TextInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TextFrom");

                    b.Property<string>("TextSecret");

                    b.Property<string>("TextToken");

                    b.Property<string>("TextUserId");

                    b.HasKey("ID");

                    b.ToTable("TextInfo");
                });

            modelBuilder.Entity("RemindMe.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CellPhoneNumber");

                    b.Property<string>("Email");

                    b.Property<string>("GCalEmail");

                    b.Property<string>("GCalEmailPassword");

                    b.Property<string>("Password");

                    b.Property<string>("UserCreateDate");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RemindMe.Models.NonRecurringEvents", b =>
                {
                    b.HasOne("RemindMe.Models.User", "User")
                        .WithMany("NonRecurringEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RemindMe.Models.NonRecurringReminders", b =>
                {
                    b.HasOne("RemindMe.Models.User", "User")
                        .WithMany("NonRecurringReminders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RemindMe.Models.RecurringEvents", b =>
                {
                    b.HasOne("RemindMe.Models.User", "User")
                        .WithMany("RecurringEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RemindMe.Models.RecurringReminders", b =>
                {
                    b.HasOne("RemindMe.Models.ReminderRepeatFrequencies", "RepeatFrequencyName")
                        .WithMany("Reminders")
                        .HasForeignKey("RepeatFrequencyNameID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RemindMe.Models.User", "User")
                        .WithMany("RecurringReminders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
