using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class LikelhoodandImpactColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Impact",
                table: "Risks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likelyhood",
                table: "Risks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "Likelyhood",
                table: "Risks");
        }
    }
}
