using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class addedprototypefortwodailyreminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FiveAm",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropColumn(
                name: "FourAm",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropColumn(
                name: "Midnight",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropColumn(
                name: "OneAm",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropColumn(
                name: "ThreeAm",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.RenameColumn(
                name: "TwoAm",
                table: "SendRemindersMidnightToFiveAm",
                newName: "TimeToSendReminder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeToSendReminder",
                table: "SendRemindersMidnightToFiveAm",
                newName: "TwoAm");

            migrationBuilder.AddColumn<string>(
                name: "FiveAm",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FourAm",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Midnight",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneAm",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreeAm",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);
        }
    }
}
