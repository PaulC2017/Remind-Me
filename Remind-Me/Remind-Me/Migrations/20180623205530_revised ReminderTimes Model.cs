using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class revisedReminderTimesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eightAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "eightPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "elevenAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "elevenPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "fiveAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "fivePm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "fourAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "fourPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "midnight",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "nineAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "ninePm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "noon",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "oneAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "onePm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "sevenAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "sevenPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "sixAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "sixPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "skipThisOne",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "tenAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "tenPm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "threeAm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "threePm",
                table: "ReminderTimes");

            migrationBuilder.DropColumn(
                name: "twoAm",
                table: "ReminderTimes");

            migrationBuilder.RenameColumn(
                name: "twoPm",
                table: "ReminderTimes",
                newName: "ReminderTimesName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReminderTimesName",
                table: "ReminderTimes",
                newName: "twoPm");

            migrationBuilder.AddColumn<string>(
                name: "eightAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "eightPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "elevenAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "elevenPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fiveAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fivePm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fourAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fourPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "midnight",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nineAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ninePm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "noon",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oneAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "onePm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sevenAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sevenPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sixAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sixPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "skipThisOne",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenPm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "threeAm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "threePm",
                table: "ReminderTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "twoAm",
                table: "ReminderTimes",
                nullable: true);
        }
    }
}
