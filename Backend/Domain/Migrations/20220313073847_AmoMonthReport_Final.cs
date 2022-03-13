using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class AmoMonthReport_Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfSalesNextMonth",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountOfSalesThisMonth",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameCompletedThisMonth",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GamesSoldNextMonth",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GamesSoldThisMonth",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfSalesNextMonth",
                table: "AmoMonthReports");

            migrationBuilder.DropColumn(
                name: "AmountOfSalesThisMonth",
                table: "AmoMonthReports");

            migrationBuilder.DropColumn(
                name: "GameCompletedThisMonth",
                table: "AmoMonthReports");

            migrationBuilder.DropColumn(
                name: "GamesSoldNextMonth",
                table: "AmoMonthReports");

            migrationBuilder.DropColumn(
                name: "GamesSoldThisMonth",
                table: "AmoMonthReports");
        }
    }
}
