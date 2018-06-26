using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class updatedDBContextforReminderTimesver2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReminderTimesID",
                table: "RecurringReminders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReminderTimes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkipThisOne = table.Column<string>(nullable: true),
                    OneAm = table.Column<string>(nullable: true),
                    TwoAm = table.Column<string>(nullable: true),
                    ThreeAm = table.Column<string>(nullable: true),
                    FourAm = table.Column<string>(nullable: true),
                    FiveAm = table.Column<string>(nullable: true),
                    SixAm = table.Column<string>(nullable: true),
                    SevenAm = table.Column<string>(nullable: true),
                    EightAm = table.Column<string>(nullable: true),
                    NineAm = table.Column<string>(nullable: true),
                    TenAm = table.Column<string>(nullable: true),
                    ElevenAm = table.Column<string>(nullable: true),
                    Noon = table.Column<string>(nullable: true),
                    OnePM = table.Column<string>(nullable: true),
                    TwoPm = table.Column<string>(nullable: true),
                    ThreePm = table.Column<string>(nullable: true),
                    FourPm = table.Column<string>(nullable: true),
                    FivePm = table.Column<string>(nullable: true),
                    SixPm = table.Column<string>(nullable: true),
                    EightPm = table.Column<string>(nullable: true),
                    NinePm = table.Column<string>(nullable: true),
                    TenPm = table.Column<string>(nullable: true),
                    ElevenPm = table.Column<string>(nullable: true),
                    Midnight = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTimes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_ReminderTimesID",
                table: "RecurringReminders",
                column: "ReminderTimesID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesID",
                table: "RecurringReminders",
                column: "ReminderTimesID",
                principalTable: "ReminderTimes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringReminders_ReminderTimes_ReminderTimesID",
                table: "RecurringReminders");

            migrationBuilder.DropTable(
                name: "ReminderTimes");

            migrationBuilder.DropIndex(
                name: "IX_RecurringReminders_ReminderTimesID",
                table: "RecurringReminders");

            migrationBuilder.DropColumn(
                name: "ReminderTimesID",
                table: "RecurringReminders");
        }
    }
}
