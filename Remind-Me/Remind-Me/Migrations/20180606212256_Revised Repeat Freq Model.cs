using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class RevisedRepeatFreqModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderRepeatFrequencies_RepeatFrequencyNameID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "RecurringReminderRepeatFrequencyID",
                table: "RecurringReminders");

            migrationBuilder.AlterColumn<int>(
                name: "RepeatFrequencyNameID",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderRepeatFrequencies_RepeatFrequencyNameID",
                table: "RecurringReminders",
                column: "RepeatFrequencyNameID",
                principalTable: "ReminderRepeatFrequencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderRepeatFrequencies_RepeatFrequencyNameID",
                table: "RecurringReminders");

            migrationBuilder.AlterColumn<int>(
                name: "RepeatFrequencyNameID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RecurringReminderRepeatFrequencyID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderRepeatFrequencies_RepeatFrequencyNameID",
                table: "RecurringReminders",
                column: "RepeatFrequencyNameID",
                principalTable: "ReminderRepeatFrequencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
