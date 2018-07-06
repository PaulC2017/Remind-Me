using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class updatedsendremindertimexxxxxmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropColumn(
                name: "RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropColumn(
                name: "RecurringReminderId",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropColumn(
                name: "RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurringReminderId",
                table: "SendRemindersNoonToFivePm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm",
                nullable: false,
                defaultValue: 0);
        }
    }
}
