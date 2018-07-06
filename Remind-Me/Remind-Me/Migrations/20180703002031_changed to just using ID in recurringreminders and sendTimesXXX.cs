using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class changedtojustusingIDinrecurringremindersandsendTimesXXX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminder",
                table: "SendRemindersSixPmToElevenPm",
                newName: "TimeToSendReminderSTEPM");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminder",
                table: "SendRemindersSixAmToElevenAm",
                newName: "TimeToSendReminderSTEAM");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminder",
                table: "SendRemindersNoonToFivePm",
                newName: "TimeToSendReminderNTFPM");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminder",
                table: "SendRemindersMidnightToFiveAm",
                newName: "TimeToSendReminderMTFAM");

            migrationBuilder.AddColumn<int>(
                name: "TimeToSendReminderMTFAMID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeToSendReminderNTFPMID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeToSendReminderSTEAMID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeToSendReminderSTEPMID",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "TimeToSendReminderMTFAMID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "TimeToSendReminderNTFPMID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "TimeToSendReminderSTEAMID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "TimeToSendReminderSTEPMID",
                table: "RecurringReminders");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminderSTEPM",
                table: "SendRemindersSixPmToElevenPm",
                newName: "TimeToSendReminder");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminderSTEAM",
                table: "SendRemindersSixAmToElevenAm",
                newName: "TimeToSendReminder");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminderNTFPM",
                table: "SendRemindersNoonToFivePm",
                newName: "TimeToSendReminder");

            migrationBuilder.RenameColumn(
                name: "TimeToSendReminderMTFAM",
                table: "SendRemindersMidnightToFiveAm",
                newName: "TimeToSendReminder");

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
    }
}
