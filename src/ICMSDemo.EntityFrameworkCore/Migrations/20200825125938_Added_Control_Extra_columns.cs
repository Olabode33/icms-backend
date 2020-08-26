using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Control_Extra_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ControlObjective",
                table: "Controls",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ControlOwnerId",
                table: "Controls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControlPoint",
                table: "Controls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Controls_ControlOwnerId",
                table: "Controls",
                column: "ControlOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Controls_AbpUsers_ControlOwnerId",
                table: "Controls",
                column: "ControlOwnerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controls_AbpUsers_ControlOwnerId",
                table: "Controls");

            migrationBuilder.DropIndex(
                name: "IX_Controls_ControlOwnerId",
                table: "Controls");

            migrationBuilder.DropColumn(
                name: "ControlObjective",
                table: "Controls");

            migrationBuilder.DropColumn(
                name: "ControlOwnerId",
                table: "Controls");

            migrationBuilder.DropColumn(
                name: "ControlPoint",
                table: "Controls");
        }
    }
}
