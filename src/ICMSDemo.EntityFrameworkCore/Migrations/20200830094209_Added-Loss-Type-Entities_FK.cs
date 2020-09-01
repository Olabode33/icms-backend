using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class AddedLossTypeEntities_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LossTypeColumns_LossTypeId",
                table: "LossTypeColumns",
                column: "LossTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LossTypeColumns_LossTypes_LossTypeId",
                table: "LossTypeColumns",
                column: "LossTypeId",
                principalTable: "LossTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LossTypeColumns_LossTypes_LossTypeId",
                table: "LossTypeColumns");

            migrationBuilder.DropIndex(
                name: "IX_LossTypeColumns_LossTypeId",
                table: "LossTypeColumns");
        }
    }
}
