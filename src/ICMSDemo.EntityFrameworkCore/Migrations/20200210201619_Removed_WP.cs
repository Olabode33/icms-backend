using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Removed_WP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPaperNews");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "WorkingPapers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "WorkingPapers");

            migrationBuilder.CreateTable(
                name: "WorkingPaperNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedUserId = table.Column<long>(type: "bigint", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaskDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TestingTemplateId = table.Column<int>(type: "int", nullable: true)
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
    }
}
