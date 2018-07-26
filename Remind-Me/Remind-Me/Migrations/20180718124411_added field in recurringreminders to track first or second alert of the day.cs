using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class addedfieldinrecurringreminderstotrackfirstorsecondalertoftheday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstOrSecondAlertOfTheDay",
                table: "RecurringReminders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstOrSecondAlertOfTheDay",
                table: "RecurringReminders");
        }
    }
}
