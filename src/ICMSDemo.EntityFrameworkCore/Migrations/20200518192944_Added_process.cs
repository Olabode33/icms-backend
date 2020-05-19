using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_process : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Casade",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Process_Description",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Process_Name",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_DepartmentId",
                table: "AbpOrganizationUnits",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_OwnerId",
                table: "AbpOrganizationUnits",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId1",
                table: "AbpOrganizationUnits",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_DepartmentId",
                table: "AbpOrganizationUnits",
                column: "DepartmentId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_OwnerId",
                table: "AbpOrganizationUnits",
                column: "OwnerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_DepartmentId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_OwnerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_DepartmentId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_OwnerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId1",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Casade",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Process_Description",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Process_Name",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "AbpOrganizationUnits");
        }
    }
}
