using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class addedthefulltworemindersperdaycode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SendRemindersNoonToFivePm",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeToSendReminder = table.Column<string>(nullable: true),
                    RecurringReminderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendRemindersNoonToFivePm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SendRemindersNoonToFivePm_RecurringReminders_RecurringReminderId",
                        column: x => x.RecurringReminderId,
                        principalTable: "RecurringReminders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SendRemindersSixAmToElevenAm",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeToSendReminder = table.Column<string>(nullable: true),
                    RecurringReminderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendRemindersSixAmToElevenAm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SendRemindersSixAmToElevenAm_RecurringReminders_RecurringReminderId",
                        column: x => x.RecurringReminderId,
                        principalTable: "RecurringReminders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SendRemindersSixPmToElevenPm",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeToSendReminder = table.Column<string>(nullable: true),
                    RecurringReminderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendRemindersSixPmToElevenPm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SendRemindersSixPmToElevenPm_RecurringReminders_RecurringReminderId",
                        column: x => x.RecurringReminderId,
                        principalTable: "RecurringReminders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmID",
                table: "RecurringReminders",
                column: "SendRemindersNoonToFivePmID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders",
                column: "SendRemindersSixAmToElevenAmID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders",
                column: "SendRemindersSixPmToElevenPmID");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersNoonToFivePm_RecurringReminderId",
                table: "SendRemindersNoonToFivePm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixAmToElevenAm_RecurringReminderId",
                table: "SendRemindersSixAmToElevenAm",
                column: "RecurringReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersSixPmToElevenPm_RecurringReminderId",
                table: "SendRemindersSixPmToElevenPm",
                column: "RecurringReminderId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersNoonToFivePm_SendRemindersNoonToFivePmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixAmToElevenAm_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersSixPmToElevenPm_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders");

            migrationBuilder.DropTable(
                name: "SendRemindersNoonToFivePm");

            migrationBuilder.DropTable(
                name: "SendRemindersSixAmToElevenAm");

            migrationBuilder.DropTable(
                name: "SendRemindersSixPmToElevenPm");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersNoonToFivePmID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersNoonToFivePmID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersSixAmToElevenAmID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersSixPmToElevenPmID",
                table: "RecurringReminders");
        }
    }
}
