using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_ExceptionIncidentCOlumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionIncidents_TestingTemplates_TestingTemplateId",
                table: "ExceptionIncidents");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionIncidents_TestingTemplateId",
                table: "ExceptionIncidents");

            migrationBuilder.DropColumn(
                name: "TestingTemplateId",
                table: "ExceptionIncidents");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkingPaperFkId",
                table: "ExceptionIncidents",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExceptionIncidentColumns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    ExceptionIncidentId = table.Column<int>(nullable: true),
                    ExceptionTypeColumnId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionIncidentColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidentColumns_ExceptionIncidents_ExceptionIncidentId",
                        column: x => x.ExceptionIncidentId,
                        principalTable: "ExceptionIncidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidentColumns_ExceptionTypeColumns_ExceptionTypeColumnId",
                        column: x => x.ExceptionTypeColumnId,
                        principalTable: "ExceptionTypeColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_WorkingPaperFkId",
                table: "ExceptionIncidents",
                column: "WorkingPaperFkId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidentColumns_ExceptionIncidentId",
                table: "ExceptionIncidentColumns",
                column: "ExceptionIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidentColumns_ExceptionTypeColumnId",
                table: "ExceptionIncidentColumns",
                column: "ExceptionTypeColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionIncidents_WorkingPapers_WorkingPaperFkId",
                table: "ExceptionIncidents",
                column: "WorkingPaperFkId",
                principalTable: "WorkingPapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionIncidents_WorkingPapers_WorkingPaperFkId",
                table: "ExceptionIncidents");

            migrationBuilder.DropTable(
                name: "ExceptionIncidentColumns");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionIncidents_WorkingPaperFkId",
                table: "ExceptionIncidents");

            migrationBuilder.DropColumn(
                name: "WorkingPaperFkId",
                table: "ExceptionIncidents");

            migrationBuilder.AddColumn<int>(
                name: "TestingTemplateId",
                table: "ExceptionIncidents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_TestingTemplateId",
                table: "ExceptionIncidents",
                column: "TestingTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionIncidents_TestingTemplates_TestingTemplateId",
                table: "ExceptionIncidents",
                column: "TestingTemplateId",
                principalTable: "TestingTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
