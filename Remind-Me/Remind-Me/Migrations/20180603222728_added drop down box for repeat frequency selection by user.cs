using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindMe.Migrations
{
    public partial class addeddropdownboxforrepeatfrequencyselectionbyuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReminderRepeatFrequencies",
                columns: table => new
                {
                    RepeatFrequencyName = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderRepeatFrequencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TextInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TextUserId = table.Column<string>(nullable: true),
                    TextToken = table.Column<string>(nullable: true),
                    TextSecret = table.Column<string>(nullable: true),
                    TextFrom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    GCalEmail = table.Column<string>(nullable: true),
                    GCalEmailPassword = table.Column<string>(nullable: true),
                    CellPhoneNumber = table.Column<string>(nullable: true),
                    UserCreateDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NonRecurringEvents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NonRecurringEventName = table.Column<string>(nullable: true),
                    NonRecurringEventDescription = table.Column<string>(nullable: true),
                    NonRecuringEventCreateDate = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRecurringEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NonRecurringEvents_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonRecurringReminders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NonRecurringReminderName = table.Column<string>(nullable: true),
                    NonRecurringReminderDescription = table.Column<string>(nullable: true),
                    NonRecuringReminderCreateDate = table.Column<string>(nullable: true),
                    NonRecurringEventDate = table.Column<DateTime>(nullable: false),
                    NonRecurringEventTime = table.Column<string>(nullable: true),
                    NonRecurringReminderStartAlertDate = table.Column<DateTime>(nullable: false),
                    NonRecurringReminderLastAlertDate = table.Column<DateTime>(nullable: false),
                    NonRecurringReminderFirstAlertTime = table.Column<string>(nullable: true),
                    NonRecurringReminderSecondAlertTime = table.Column<string>(nullable: true),
                    NonRecuringReminderAlertFrequency = table.Column<string>(nullable: true),
                    UserCellPhoneNumber = table.Column<string>(nullable: true),
                    NonRecurringReminderDateAndTimeLastAlertSent = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRecurringReminders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NonRecurringReminders_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecurringEvents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecurringEventName = table.Column<string>(nullable: true),
                    RecurringEventDescription = table.Column<string>(nullable: true),
                    RecurringEventDate = table.Column<DateTime>(nullable: false),
                    RecuringEventCreateDate = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecurringEvents_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecurringReminders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecurringReminderName = table.Column<string>(nullable: true),
                    RecurringReminderDescription = table.Column<string>(nullable: true),
                    RecurringEventDate = table.Column<DateTime>(nullable: false),
                    RecuringReminderCreateDate = table.Column<string>(nullable: true),
                    RecurringReminderStartAlertDate = table.Column<DateTime>(nullable: false),
                    RecurringReminderLastAlertDate = table.Column<DateTime>(nullable: false),
                    RecurringReminderFirstAlertTime = table.Column<string>(nullable: true),
                    RecurringReminderSecondAlertTime = table.Column<string>(nullable: true),
                    RecurringReminderRepeatFrequencyID = table.Column<int>(nullable: false),
                    RepeatFrequencyNameID = table.Column<int>(nullable: true),
                    UserCellPhoneNumber = table.Column<string>(nullable: true),
                    RecurringReminderDateAndTimeLastAlertSent = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringReminders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecurringReminders_ReminderRepeatFrequencies_RepeatFrequencyNameID",
                        column: x => x.RepeatFrequencyNameID,
                        principalTable: "ReminderRepeatFrequencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringReminders_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonRecurringEvents_UserId",
                table: "NonRecurringEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NonRecurringReminders_UserId",
                table: "NonRecurringReminders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringEvents_UserId",
                table: "RecurringEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_RepeatFrequencyNameID",
                table: "RecurringReminders",
                column: "RepeatFrequencyNameID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReminders_UserId",
                table: "RecurringReminders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonRecurringEvents");

            migrationBuilder.DropTable(
                name: "NonRecurringReminders");

            migrationBuilder.DropTable(
                name: "RecurringEvents");

            migrationBuilder.DropTable(
                name: "RecurringReminders");

            migrationBuilder.DropTable(
                name: "TextInfo");

            migrationBuilder.DropTable(
                name: "ReminderRepeatFrequencies");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
