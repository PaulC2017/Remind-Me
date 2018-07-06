using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class morechnagestoSendReminderTimesandRecurringRemindersmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_SendRemindersNoonToFivePmId",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmId",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersNoonToFivePmId",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                column: "SendRemindersMidnightToFiveAmId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                column: "SendRemindersNoonToFivePmId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                column: "SendRemindersSixAmToElevenAmId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                column: "SendRemindersSixPmToElevenPmId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                column: "SendRemindersMidnightToFiveAmId",
                principalTable: "SendRemindersMidnightToFiveAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                column: "SendRemindersNoonToFivePmId",
                principalTable: "SendRemindersNoonToFivePm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                column: "SendRemindersSixAmToElevenAmId",
                principalTable: "SendRemindersSixAmToElevenAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                column: "SendRemindersSixPmToElevenPmId",
                principalTable: "SendRemindersSixPmToElevenPm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
