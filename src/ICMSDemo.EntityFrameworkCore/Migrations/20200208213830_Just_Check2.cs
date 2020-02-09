using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Just_Check2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUserOrganizationUnits",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentRole",
                table: "AbpUserOrganizationUnits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUserOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "DepartmentRole",
                table: "AbpUserOrganizationUnits");
        }
    }
}
