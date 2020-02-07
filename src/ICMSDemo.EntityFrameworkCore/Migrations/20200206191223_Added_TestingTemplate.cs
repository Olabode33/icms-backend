using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_TestingTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cascade",
                table: "DepartmentRisks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cascade",
                table: "DepartmentRiskControls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TestingTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    DetailedInstructions = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Frequency = table.Column<int>(nullable: false),
                    DepartmentRiskControlId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingTemplates_DepartmentRiskControls_DepartmentRiskControlId",
                        column: x => x.DepartmentRiskControlId,
                        principalTable: "DepartmentRiskControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestingTemplates_DepartmentRiskControlId",
                table: "TestingTemplates",
                column: "DepartmentRiskControlId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingTemplates_TenantId",
                table: "TestingTemplates",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestingTemplates");

            migrationBuilder.DropColumn(
                name: "Cascade",
                table: "DepartmentRisks");

            migrationBuilder.DropColumn(
                name: "Cascade",
                table: "DepartmentRiskControls");
        }
    }
}
