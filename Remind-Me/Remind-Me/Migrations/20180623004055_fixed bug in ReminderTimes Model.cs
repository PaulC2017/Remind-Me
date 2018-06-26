using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class fixedbuginReminderTimesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoPm",
                table: "ReminderTimes",
                newName: "twoPm");

            migrationBuilder.RenameColumn(
                name: "TwoAm",
                table: "ReminderTimes",
                newName: "twoAm");

            migrationBuilder.RenameColumn(
                name: "ThreePm",
                table: "ReminderTimes",
                newName: "threePm");

            migrationBuilder.RenameColumn(
                name: "ThreeAm",
                table: "ReminderTimes",
                newName: "threeAm");

            migrationBuilder.RenameColumn(
                name: "TenPm",
                table: "ReminderTimes",
                newName: "tenPm");

            migrationBuilder.RenameColumn(
                name: "TenAm",
                table: "ReminderTimes",
                newName: "tenAm");

            migrationBuilder.RenameColumn(
                name: "SkipThisOne",
                table: "ReminderTimes",
                newName: "skipThisOne");

            migrationBuilder.RenameColumn(
                name: "SixPm",
                table: "ReminderTimes",
                newName: "sixPm");

            migrationBuilder.RenameColumn(
                name: "SixAm",
                table: "ReminderTimes",
                newName: "sixAm");

            migrationBuilder.RenameColumn(
                name: "SevenPm",
                table: "ReminderTimes",
                newName: "sevenPm");

            migrationBuilder.RenameColumn(
                name: "SevenAm",
                table: "ReminderTimes",
                newName: "sevenAm");

            migrationBuilder.RenameColumn(
                name: "OnePm",
                table: "ReminderTimes",
                newName: "onePm");

            migrationBuilder.RenameColumn(
                name: "OneAm",
                table: "ReminderTimes",
                newName: "oneAm");

            migrationBuilder.RenameColumn(
                name: "Noon",
                table: "ReminderTimes",
                newName: "noon");

            migrationBuilder.RenameColumn(
                name: "NinePm",
                table: "ReminderTimes",
                newName: "ninePm");

            migrationBuilder.RenameColumn(
                name: "NineAm",
                table: "ReminderTimes",
                newName: "nineAm");

            migrationBuilder.RenameColumn(
                name: "Midnight",
                table: "ReminderTimes",
                newName: "midnight");

            migrationBuilder.RenameColumn(
                name: "FourPm",
                table: "ReminderTimes",
                newName: "fourPm");

            migrationBuilder.RenameColumn(
                name: "FourAm",
                table: "ReminderTimes",
                newName: "fourAm");

            migrationBuilder.RenameColumn(
                name: "FivePm",
                table: "ReminderTimes",
                newName: "fivePm");

            migrationBuilder.RenameColumn(
                name: "FiveAm",
                table: "ReminderTimes",
                newName: "fiveAm");

            migrationBuilder.RenameColumn(
                name: "ElevenPm",
                table: "ReminderTimes",
                newName: "elevenPm");

            migrationBuilder.RenameColumn(
                name: "ElevenAm",
                table: "ReminderTimes",
                newName: "elevenAm");

            migrationBuilder.RenameColumn(
                name: "EightPm",
                table: "ReminderTimes",
                newName: "eightPm");

            migrationBuilder.RenameColumn(
                name: "EightAm",
                table: "ReminderTimes",
                newName: "eightAm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "twoPm",
                table: "ReminderTimes",
                newName: "TwoPm");

            migrationBuilder.RenameColumn(
                name: "twoAm",
                table: "ReminderTimes",
                newName: "TwoAm");

            migrationBuilder.RenameColumn(
                name: "threePm",
                table: "ReminderTimes",
                newName: "ThreePm");

            migrationBuilder.RenameColumn(
                name: "threeAm",
                table: "ReminderTimes",
                newName: "ThreeAm");

            migrationBuilder.RenameColumn(
                name: "tenPm",
                table: "ReminderTimes",
                newName: "TenPm");

            migrationBuilder.RenameColumn(
                name: "tenAm",
                table: "ReminderTimes",
                newName: "TenAm");

            migrationBuilder.RenameColumn(
                name: "skipThisOne",
                table: "ReminderTimes",
                newName: "SkipThisOne");

            migrationBuilder.RenameColumn(
                name: "sixPm",
                table: "ReminderTimes",
                newName: "SixPm");

            migrationBuilder.RenameColumn(
                name: "sixAm",
                table: "ReminderTimes",
                newName: "SixAm");

            migrationBuilder.RenameColumn(
                name: "sevenPm",
                table: "ReminderTimes",
                newName: "SevenPm");

            migrationBuilder.RenameColumn(
                name: "sevenAm",
                table: "ReminderTimes",
                newName: "SevenAm");

            migrationBuilder.RenameColumn(
                name: "onePm",
                table: "ReminderTimes",
                newName: "OnePm");

            migrationBuilder.RenameColumn(
                name: "oneAm",
                table: "ReminderTimes",
                newName: "OneAm");

            migrationBuilder.RenameColumn(
                name: "noon",
                table: "ReminderTimes",
                newName: "Noon");

            migrationBuilder.RenameColumn(
                name: "ninePm",
                table: "ReminderTimes",
                newName: "NinePm");

            migrationBuilder.RenameColumn(
                name: "nineAm",
                table: "ReminderTimes",
                newName: "NineAm");

            migrationBuilder.RenameColumn(
                name: "midnight",
                table: "ReminderTimes",
                newName: "Midnight");

            migrationBuilder.RenameColumn(
                name: "fourPm",
                table: "ReminderTimes",
                newName: "FourPm");

            migrationBuilder.RenameColumn(
                name: "fourAm",
                table: "ReminderTimes",
                newName: "FourAm");

            migrationBuilder.RenameColumn(
                name: "fivePm",
                table: "ReminderTimes",
                newName: "FivePm");

            migrationBuilder.RenameColumn(
                name: "fiveAm",
                table: "ReminderTimes",
                newName: "FiveAm");

            migrationBuilder.RenameColumn(
                name: "elevenPm",
                table: "ReminderTimes",
                newName: "ElevenPm");

            migrationBuilder.RenameColumn(
                name: "elevenAm",
                table: "ReminderTimes",
                newName: "ElevenAm");

            migrationBuilder.RenameColumn(
                name: "eightPm",
                table: "ReminderTimes",
                newName: "EightPm");

            migrationBuilder.RenameColumn(
                name: "eightAm",
                table: "ReminderTimes",
                newName: "EightAm");
        }
    }
}
