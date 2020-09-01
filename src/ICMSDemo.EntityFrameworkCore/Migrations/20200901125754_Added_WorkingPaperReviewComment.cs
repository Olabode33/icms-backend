using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_WorkingPaperReviewComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingPaperReviewComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    CompletionDate = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Severity = table.Column<int>(nullable: false),
                    ExpectedCompletionDate = table.Column<DateTime>(nullable: false),
                    AssigneeUserId = table.Column<long>(nullable: true),
                    WorkingPaperId = table.Column<Guid>(nullable: true),
                    AssignerUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPaperReviewComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingPaperReviewComments_AbpUsers_AssigneeUserId",
                        column: x => x.AssigneeUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperReviewComments_AbpUsers_AssignerUserId",
                        column: x => x.AssignerUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperReviewComments_WorkingPapers_WorkingPaperId",
                        column: x => x.WorkingPaperId,
                        principalTable: "WorkingPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperReviewComments_AssigneeUserId",
                table: "WorkingPaperReviewComments",
                column: "AssigneeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperReviewComments_AssignerUserId",
                table: "WorkingPaperReviewComments",
                column: "AssignerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperReviewComments_TenantId",
                table: "WorkingPaperReviewComments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperReviewComments_WorkingPaperId",
                table: "WorkingPaperReviewComments",
                column: "WorkingPaperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPaperReviewComments");
        }
    }
}
