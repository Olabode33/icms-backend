using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class REmoved_Control_Officer_From_Dept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ControlOfficerUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_ControlOfficerUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ControlOfficerUserId",
                table: "AbpOrganizationUnits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                column: "ControlOfficerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                column: "ControlOfficerUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
