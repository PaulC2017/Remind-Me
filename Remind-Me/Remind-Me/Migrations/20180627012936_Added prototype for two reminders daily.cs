using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class Addedprototypefortworemindersdaily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SendRemindersMidnightToFiveAm",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Midnight = table.Column<string>(nullable: true),
                    OneAm = table.Column<string>(nullable: true),
                    TwoAm = table.Column<string>(nullable: true),
                    ThreeAm = table.Column<string>(nullable: true),
                    FourAm = table.Column<string>(nullable: true),
                    FiveAm = table.Column<string>(nullable: true),
                    RecurringReminderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendRemindersMidnightToFiveAm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SendRemindersMidnightToFiveAm_RecurringReminders_RecurringReminderId",
                        column: x => x.RecurringReminderId,
                        principalTable: "RecurringReminders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                column: "SendRemindersMidnightToFiveAmID");

            migrationBuilder.CreateIndex(
                name: "IX_SendRemindersMidnightToFiveAm_RecurringReminderId",
                table: "SendRemindersMidnightToFiveAm",
                column: "RecurringReminderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders",
                column: "SendRemindersMidnightToFiveAmID",
                principalTable: "SendRemindersMidnightToFiveAm",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_SendRemindersMidnightToFiveAm_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders");

            migrationBuilder.DropTable(
                name: "SendRemindersMidnightToFiveAm");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "SendRemindersMidnightToFiveAmID",
                table: "RecurringReminders");
        }
    }
}
