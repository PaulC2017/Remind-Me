using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class moremodificationstoRecurringRemindersandSendreminderTimesXXXXX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringRemindersID",
                table: "SendRemindersNoonToFivePm",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixPmToElevenPm_RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm",
                column: "RecurringRemindersID");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixAmToElevenAm_RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm",
                column: "RecurringRemindersID");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersNoonToFivePm_RecurringRemindersID",
                table: "SendRemindersNoonToFivePm",
                column: "RecurringRemindersID");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersMidnightToFiveAm_RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm",
                column: "RecurringRemindersID");

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersMidnightToFiveAm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm",
                column: "RecurringRemindersID",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersNoonToFivePm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersNoonToFivePm",
                column: "RecurringRemindersID",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersSixAmToElevenAm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm",
                column: "RecurringRemindersID",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersSixPmToElevenPm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm",
                column: "RecurringRemindersID",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersMidnightToFiveAm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersNoonToFivePm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersSixAmToElevenAm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersSixPmToElevenPm_RecurringReminders_RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersSixPmToElevenPm_RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersSixAmToElevenAm_RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersNoonToFivePm_RecurringRemindersID",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersMidnightToFiveAm_RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropColumn(
                name: "RecurringRemindersID",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropColumn(
                name: "RecurringRemindersID",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropColumn(
                name: "RecurringRemindersID",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropColumn(
                name: "RecurringRemindersID",
                table: "SendRemindersMidnightToFiveAm");
        }
    }
}
