using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_DepartmentRisk_Extra_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Impact",
                table: "DepartmentRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likelyhood",
                table: "DepartmentRisks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Impact",
                table: "DepartmentRisks");

            migrationBuilder.DropColumn(
                name: "Likelyhood",
                table: "DepartmentRisks");
        }
    }
}
