using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class Repoerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "AmoMonthReports",
                newName: "Sales");

            migrationBuilder.RenameColumn(
                name: "IncorrectSum",
                table: "AmoMonthReports",
                newName: "Leads");

            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "AmoDayReports",
                newName: "Sales");

            migrationBuilder.RenameColumn(
                name: "IncorrectSum",
                table: "AmoDayReports",
                newName: "Overdue");

            migrationBuilder.AddColumn<int>(
                name: "Closed",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Closed",
                table: "AmoDayReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Forehead",
                table: "AmoDayReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Leads",
                table: "AmoDayReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "AmoMonthReports");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "AmoDayReports");

            migrationBuilder.DropColumn(
                name: "Forehead",
                table: "AmoDayReports");

            migrationBuilder.DropColumn(
                name: "Leads",
                table: "AmoDayReports");

            migrationBuilder.RenameColumn(
                name: "Sales",
                table: "AmoMonthReports",
                newName: "Sum");

            migrationBuilder.RenameColumn(
                name: "Leads",
                table: "AmoMonthReports",
                newName: "IncorrectSum");

            migrationBuilder.RenameColumn(
                name: "Sales",
                table: "AmoDayReports",
                newName: "Sum");

            migrationBuilder.RenameColumn(
                name: "Overdue",
                table: "AmoDayReports",
                newName: "IncorrectSum");
        }
    }
}
