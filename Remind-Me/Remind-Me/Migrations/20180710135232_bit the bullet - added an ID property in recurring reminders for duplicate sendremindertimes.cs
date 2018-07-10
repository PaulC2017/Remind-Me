using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class bitthebulletaddedanIDpropertyinrecurringremindersforduplicatesendremindertimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeToSendReminderDuplicatesID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToSendReminderDuplicatesID",
                table: "RecurringReminders");
        }
    }
}
