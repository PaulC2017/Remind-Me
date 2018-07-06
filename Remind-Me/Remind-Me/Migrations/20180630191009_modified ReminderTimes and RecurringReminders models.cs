using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class modifiedReminderTimesandRecurringRemindersmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesID",
                table: "RecurringReminders");

            migrationBuilder.RenameColumn(
                name: "ReminderTimesID",
                table: "RecurringReminders",
                newName: "ReminderTimesId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_ReminderTimesID",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_ReminderTimesId");

            migrationBuilder.AlterColumn<int>(
                name: "ReminderTimesId",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesId",
                table: "RecurringReminders",
                column: "ReminderTimesId",
                principalTable: "ReminderTimes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesId",
                table: "RecurringReminders");

            migrationBuilder.RenameColumn(
                name: "ReminderTimesId",
                table: "RecurringReminders",
                newName: "ReminderTimesID");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_ReminderTimesId",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_ReminderTimesID");

            migrationBuilder.AlterColumn<int>(
                name: "ReminderTimesID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesID",
                table: "RecurringReminders",
                column: "ReminderTimesID",
                principalTable: "ReminderTimes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
