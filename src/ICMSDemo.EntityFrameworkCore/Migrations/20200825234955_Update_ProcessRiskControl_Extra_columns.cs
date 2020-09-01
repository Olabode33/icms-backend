using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Update_ProcessRiskControl_Extra_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Likelyhood",
                table: "DepartmentRiskControls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Impact",
                table: "DepartmentRiskControls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Likelyhood",
                table: "DepartmentRiskControls",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Impact",
                table: "DepartmentRiskControls",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
