using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Udpated_ControlTesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssignedUserId",
                table: "ControlTestings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlTestings_AssignedUserId",
                table: "ControlTestings",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlTestings_AbpUsers_AssignedUserId",
                table: "ControlTestings",
                column: "AssignedUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlTestings_AbpUsers_AssignedUserId",
                table: "ControlTestings");

            migrationBuilder.DropIndex(
                name: "IX_ControlTestings_AssignedUserId",
                table: "ControlTestings");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "ControlTestings");
        }
    }
}
