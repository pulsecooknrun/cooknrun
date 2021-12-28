using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class AmoMonthReports2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TodaySum",
                table: "AmoDayReports",
                newName: "Sum");

            migrationBuilder.RenameColumn(
                name: "IncorrectTodaySum",
                table: "AmoDayReports",
                newName: "LeadsWithoutTasks");

            migrationBuilder.AddColumn<int>(
                name: "IncorrectSum",
                table: "AmoDayReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncorrectSum",
                table: "AmoDayReports");

            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "AmoDayReports",
                newName: "TodaySum");

            migrationBuilder.RenameColumn(
                name: "LeadsWithoutTasks",
                table: "AmoDayReports",
                newName: "IncorrectTodaySum");
        }
    }
}
