using RemindMe.Models;
using Microsoft.EntityFrameworkCore;

namespace RemindMe.Data
{
    public class RemindMeDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<RecurringReminders> RecurringReminders { get; set; }
        public DbSet<NonRecurringReminders> NonRecurringReminders { get; set; }
        public DbSet<TextInfo> TextInfo { get; set; }
        public DbSet<ReminderRepeatFrequencies> ReminderRepeatFrequencies { get; set; }


        public DbSet<RecurringEvents> RecurringEvents { get; set; }
        public DbSet<NonRecurringEvents> NonRecurringEvents { get; set; }

        public RemindMeDbContext(DbContextOptions<RemindMeDbContext> options)
              : base(options)
        { }


        /*
        This method will set the primary key of the UserEventTypesNonRecurring class and table to be a composite key, consisting of both NonRecurringEventId  and UserId.
        (Many-to-Many relationship between User and EventTypesNonRecurring  and 
        will set the primary key of the UserEventTypesRecurring class and table to be a composite key, consisting of both RecurringEventId  and UserId.
        Many-to-Many relationship between User and EventTypesRecurring  and)
                              
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRecurringReminders>()
                .HasKey(c => new {c.RecurringRemindersID, c.UserID});

            modelBuilder.Entity<UserNonRecurringReminders>()
                .HasKey(c => new { c.NonRecurringRemindersID, c.UserID });

            modelBuilder.Entity<UserRecurringEvents>()
                .HasKey(c => new { c.RecurringEventsID, c.UserID });

            modelBuilder.Entity<UserNonRecurringEvents>()
                .HasKey(c => new { c.NonRecurringEventsID, c.UserID });



        }
        */
    }
}
