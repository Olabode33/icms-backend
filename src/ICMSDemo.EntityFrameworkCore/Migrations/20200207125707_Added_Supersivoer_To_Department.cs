using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Supersivoer_To_Department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SupervisingUnitId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_SupervisingUnitId",
                table: "AbpOrganizationUnits",
                column: "SupervisingUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_SupervisingUnitId",
                table: "AbpOrganizationUnits",
                column: "SupervisingUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_SupervisingUnitId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_SupervisingUnitId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "SupervisingUnitId",
                table: "AbpOrganizationUnits");
        }
    }
}
