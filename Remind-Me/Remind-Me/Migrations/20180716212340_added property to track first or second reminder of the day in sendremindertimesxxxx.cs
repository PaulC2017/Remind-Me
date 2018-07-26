using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class addedpropertytotrackfirstorsecondreminderofthedayinsendremindertimesxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersSixPmToElevenPm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersSixAmToElevenAm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersNoonToFivePm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropColumn(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropColumn(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropColumn(
                name: "FirstOrSecondReminderOfTheDay",
                table: "SendRemindersMidnightToFiveAm");
        }
    }
}
