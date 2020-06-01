using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Control_Level : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ControlLevel",
                table: "LibraryControls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlLevel",
                table: "LibraryControls");
        }
    }
}
