using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class redoofsendremindertimesxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_TimeToSendReminderMTFAMID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_TimeToSendReminderNTFPMID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_TimeToSendReminderSTEAMID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_TimeToSendReminderSTEPMID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_TimeToSendReminderMTFAMID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_TimeToSendReminderNTFPMID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_TimeToSendReminderSTEAMID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_TimeToSendReminderSTEPMID",
                table: "RecurringReminders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_TimeToSendReminderMTFAMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderMTFAMID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_TimeToSendReminderNTFPMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderNTFPMID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_TimeToSendReminderSTEAMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderSTEAMID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_TimeToSendReminderSTEPMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderSTEPMID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_TimeToSendReminderMTFAMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderMTFAMID",
                principalTable: "SendRemindersMidnightToFiveAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_TimeToSendReminderNTFPMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderNTFPMID",
                principalTable: "SendRemindersNoonToFivePm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_TimeToSendReminderSTEAMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderSTEAMID",
                principalTable: "SendRemindersSixAmToElevenAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_TimeToSendReminderSTEPMID",
                table: "RecurringReminders",
                column: "TimeToSendReminderSTEPMID",
                principalTable: "SendRemindersSixPmToElevenPm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
