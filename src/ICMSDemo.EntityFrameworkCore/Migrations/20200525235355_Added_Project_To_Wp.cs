using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Project_To_Wp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "WorkingPapers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPapers_ProjectId",
                table: "WorkingPapers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingPapers_Projects_ProjectId",
                table: "WorkingPapers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingPapers_Projects_ProjectId",
                table: "WorkingPapers");

            migrationBuilder.DropIndex(
                name: "IX_WorkingPapers_ProjectId",
                table: "WorkingPapers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkingPapers");
        }
    }
}
