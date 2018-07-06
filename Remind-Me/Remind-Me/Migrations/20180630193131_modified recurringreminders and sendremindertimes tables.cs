using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class modifiedrecurringremindersandsendremindertimestables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesId",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_SendRemindersNoonToFivePmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersMidnightToFiveAm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersNoonToFivePm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersSixAmToElevenAm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropForeignKey(
                name: "FK_SendRemindersSixPmToElevenPm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersSixPmToElevenPm_RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersSixAmToElevenAm_RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersNoonToFivePm_RecurringReminderId",
                table: "SendRemindersNoonToFivePm");

            migrationBuilder.DropIndex(
                name: "IX_SendRemindersMidnightToFiveAm_RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_ReminderTimesId",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "ReminderTimesId",
                table: "RecurringReminders");

            migrationBuilder.RenameColumn(
                name: "SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                newName: "SendRemindersSixPmToElevenPmId");

            migrationBuilder.RenameColumn(
                name: "SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                newName: "SendRemindersSixAmToElevenAmId");

            migrationBuilder.RenameColumn(
                name: "SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                newName: "SendRemindersNoonToFivePmId");

            migrationBuilder.RenameColumn(
                name: "SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                newName: "SendRemindersMidnightToFiveAmId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersSixPmToElevenPmId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersSixAmToElevenAmId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersNoonToFivePmId");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersMidnightToFiveAmId");

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                newName: "SendRemindersSixPmToElevenPmID");

            migrationBuilder.RenameColumn(
                name: "SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                newName: "SendRemindersSixAmToElevenAmID");

            migrationBuilder.RenameColumn(
                name: "SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                newName: "SendRemindersNoonToFivePmID");

            migrationBuilder.RenameColumn(
                name: "SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                newName: "SendRemindersMidnightToFiveAmID");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmId",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersSixPmToElevenPmID");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmId",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersSixAmToElevenAmID");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmId",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersNoonToFivePmID");

            migrationBuilder.RenameIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmId",
                table: "RecurringReminders",
                newName: "IX_RecurringReminders_SendRemindersMidnightToFiveAmID");

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ReminderTimesId",
                table: "RecurringReminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixPmToElevenPm_RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixAmToElevenAm_RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersNoonToFivePm_RecurringReminderId",
                table: "SendRemindersNoonToFivePm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersMidnightToFiveAm_RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_ReminderTimesId",
                table: "RecurringReminders",
                column: "ReminderTimesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesId",
                table: "RecurringReminders",
                column: "ReminderTimesId",
                principalTable: "ReminderTimes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                column: "SendRemindersMidnightToFiveAmID",
                principalTable: "SendRemindersMidnightToFiveAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                column: "SendRemindersNoonToFivePmID",
                principalTable: "SendRemindersNoonToFivePm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                column: "SendRemindersSixAmToElevenAmID",
                principalTable: "SendRemindersSixAmToElevenAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                column: "SendRemindersSixPmToElevenPmID",
                principalTable: "SendRemindersSixPmToElevenPm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersMidnightToFiveAm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm",
                column: "RecurringReminderId",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersNoonToFivePm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersNoonToFivePm",
                column: "RecurringReminderId",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersSixAmToElevenAm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm",
                column: "RecurringReminderId",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendRemindersSixPmToElevenPm_RecurringReminders_RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm",
                column: "RecurringReminderId",
                principalTable: "RecurringReminders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
