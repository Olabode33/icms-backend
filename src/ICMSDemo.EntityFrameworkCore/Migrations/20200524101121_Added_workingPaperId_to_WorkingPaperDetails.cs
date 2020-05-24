using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_workingPaperId_to_WorkingPaperDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkingPaperId",
                table: "WorkingPaperDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperDetails_WorkingPaperId",
                table: "WorkingPaperDetails",
                column: "WorkingPaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingPaperDetails_WorkingPapers_WorkingPaperId",
                table: "WorkingPaperDetails",
                column: "WorkingPaperId",
                principalTable: "WorkingPapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingPaperDetails_WorkingPapers_WorkingPaperId",
                table: "WorkingPaperDetails");

            migrationBuilder.DropIndex(
                name: "IX_WorkingPaperDetails_WorkingPaperId",
                table: "WorkingPaperDetails");

            migrationBuilder.DropColumn(
                name: "WorkingPaperId",
                table: "WorkingPaperDetails");
        }
    }
}
