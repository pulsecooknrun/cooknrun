using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CookRun.Domain.Migrations
{
    public partial class AmoReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AmoLeadReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadId = table.Column<int>(type: "int", nullable: false),
                    NameText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<int>(type: "int", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    MainContactId = table.Column<int>(type: "int", nullable: false),
                    MainContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainContactUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameId = table.Column<int>(type: "int", nullable: false),
                    CompanyNameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceDataName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadMark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmoLeadReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmoLeadReportTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmoLeadReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmoLeadReportTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmoLeadReportTags_AmoLeadReports_AmoLeadReportId",
                        column: x => x.AmoLeadReportId,
                        principalTable: "AmoLeadReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmoLeadReportTags_AmoLeadReportId",
                table: "AmoLeadReportTags",
                column: "AmoLeadReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmoLeadReportTags");

            migrationBuilder.DropTable(
                name: "AmoLeadReports");
        }
    }
}
