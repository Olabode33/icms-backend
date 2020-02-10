using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_WorkingPapers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingPapers",
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
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    TestingTemplateId = table.Column<int>(nullable: true),
                    CompletedById = table.Column<long>(nullable: true),
                    ReviewedById = table.Column<long>(nullable: true),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    ReviewedDate = table.Column<DateTime>(nullable: true),
                    TaskStatus = table.Column<int>(nullable: false),
                    Score = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingPapers_AbpUsers_CompletedById",
                        column: x => x.CompletedById,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPapers_AbpUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPapers_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPapers_CompletedById",
                table: "WorkingPapers",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPapers_ReviewedById",
                table: "WorkingPapers",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPapers_TestingTemplateId",
                table: "WorkingPapers",
                column: "TestingTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPapers");
        }
    }
}
