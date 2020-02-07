using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_ExceptionIncident : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionIncidents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ClosureDate = table.Column<DateTime>(nullable: true),
                    ClosureComments = table.Column<string>(nullable: true),
                    RaisedByClosureComments = table.Column<string>(nullable: true),
                    ExceptionTypeId = table.Column<int>(nullable: true),
                    RaisedById = table.Column<long>(nullable: true),
                    TestingTemplateId = table.Column<int>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidents_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidents_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidents_AbpUsers_RaisedById",
                        column: x => x.RaisedById,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExceptionIncidents_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_ExceptionTypeId",
                table: "ExceptionIncidents",
                column: "ExceptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_OrganizationUnitId",
                table: "ExceptionIncidents",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_RaisedById",
                table: "ExceptionIncidents",
                column: "RaisedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_TenantId",
                table: "ExceptionIncidents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_TestingTemplateId",
                table: "ExceptionIncidents",
                column: "TestingTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionIncidents");
        }
    }
}
