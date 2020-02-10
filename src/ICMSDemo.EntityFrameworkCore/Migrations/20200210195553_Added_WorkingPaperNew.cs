using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_WorkingPaperNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPaper2s");

            migrationBuilder.CreateTable(
                name: "WorkingPaperNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    TaskDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    TaskStatus = table.Column<int>(nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    ReviewDate = table.Column<DateTime>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    TestingTemplateId = table.Column<int>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: true),
                    CompletedUserId = table.Column<long>(nullable: true),
                    ReviewedUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPaperNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingPaperNews_AbpUsers_CompletedUserId",
                        column: x => x.CompletedUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperNews_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperNews_AbpUsers_ReviewedUserId",
                        column: x => x.ReviewedUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperNews_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperNews_CompletedUserId",
                table: "WorkingPaperNews",
                column: "CompletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperNews_OrganizationUnitId",
                table: "WorkingPaperNews",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperNews_ReviewedUserId",
                table: "WorkingPaperNews",
                column: "ReviewedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperNews_TenantId",
                table: "WorkingPaperNews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperNews_TestingTemplateId",
                table: "WorkingPaperNews",
                column: "TestingTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPaperNews");

            migrationBuilder.CreateTable(
                name: "WorkingPaper2s",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPaper2s", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaper2s_TenantId",
                table: "WorkingPaper2s",
                column: "TenantId");
        }
    }
}
