using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_ProcessRisk_and_ProcessRiskControl_Extra_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Impact",
                table: "DepartmentRiskControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likelyhood",
                table: "DepartmentRiskControls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Impact",
                table: "DepartmentRiskControls");

            migrationBuilder.DropColumn(
                name: "Likelyhood",
                table: "DepartmentRiskControls");
        }
    }
}
