using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Department_Id_To_DRC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "DepartmentRiskControls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_DepartmentId",
                table: "DepartmentRiskControls",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRiskControls_AbpOrganizationUnits_DepartmentId",
                table: "DepartmentRiskControls",
                column: "DepartmentId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRiskControls_AbpOrganizationUnits_DepartmentId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRiskControls_DepartmentId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "DepartmentRiskControls");
        }
    }
}
