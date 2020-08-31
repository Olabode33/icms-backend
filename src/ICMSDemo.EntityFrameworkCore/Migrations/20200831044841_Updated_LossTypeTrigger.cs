using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Updated_LossTypeTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NotifyUserId",
                table: "LossTypeTriggers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LossTypeTriggers_NotifyUserId",
                table: "LossTypeTriggers",
                column: "NotifyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LossTypeTriggers_AbpUsers_NotifyUserId",
                table: "LossTypeTriggers",
                column: "NotifyUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LossTypeTriggers_AbpUsers_NotifyUserId",
                table: "LossTypeTriggers");

            migrationBuilder.DropIndex(
                name: "IX_LossTypeTriggers_NotifyUserId",
                table: "LossTypeTriggers");

            migrationBuilder.DropColumn(
                name: "NotifyUserId",
                table: "LossTypeTriggers");
        }
    }
}
