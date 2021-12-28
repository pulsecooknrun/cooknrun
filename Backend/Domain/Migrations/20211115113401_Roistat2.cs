using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class Roistat2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageSale",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ConversionLeadsToSales",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ConversionVisitsToLeads",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Leads",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MarketingCost",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "McLeads",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetCost",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Profit",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Revenue",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Roi",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sales",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VisitCount",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Visits",
                table: "RoistatMeanReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageSale",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "ConversionLeadsToSales",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "ConversionVisitsToLeads",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Leads",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "MarketingCost",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "McLeads",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "NetCost",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Roi",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Sales",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "VisitCount",
                table: "RoistatMeanReports");

            migrationBuilder.DropColumn(
                name: "Visits",
                table: "RoistatMeanReports");
        }
    }
}
