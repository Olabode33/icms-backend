using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Updated_loss_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LossType",
                table: "LossEvents");

            migrationBuilder.AddColumn<int>(
                name: "LossTypeId",
                table: "LossEvents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LossEvents_LossTypeId",
                table: "LossEvents",
                column: "LossTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LossEvents_LossTypes_LossTypeId",
                table: "LossEvents",
                column: "LossTypeId",
                principalTable: "LossTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LossEvents_LossTypes_LossTypeId",
                table: "LossEvents");

            migrationBuilder.DropIndex(
                name: "IX_LossEvents_LossTypeId",
                table: "LossEvents");

            migrationBuilder.DropColumn(
                name: "LossTypeId",
                table: "LossEvents");

            migrationBuilder.AddColumn<int>(
                name: "LossType",
                table: "LossEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
