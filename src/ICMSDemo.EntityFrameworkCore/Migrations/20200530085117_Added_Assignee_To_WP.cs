using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Assignee_To_WP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssignedToId",
                table: "WorkingPapers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPapers_AssignedToId",
                table: "WorkingPapers",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingPapers_AbpUsers_AssignedToId",
                table: "WorkingPapers",
                column: "AssignedToId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingPapers_AbpUsers_AssignedToId",
                table: "WorkingPapers");

            migrationBuilder.DropIndex(
                name: "IX_WorkingPapers_AssignedToId",
                table: "WorkingPapers");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "WorkingPapers");
        }
    }
}
