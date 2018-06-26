using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class AddeddefaultvalusinReminderTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnePM",
                table: "ReminderTimes",
                newName: "OnePm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnePm",
                table: "ReminderTimes",
                newName: "OnePM");
        }
    }
}
