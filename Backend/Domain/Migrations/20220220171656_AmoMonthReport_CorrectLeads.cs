using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class AmoMonthReport_CorrectLeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectLeads",
                table: "AmoMonthReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectLeads",
                table: "AmoMonthReports");
        }
    }
}
