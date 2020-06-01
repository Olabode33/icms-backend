using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_LibraryControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibraryControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Process = table.Column<string>(nullable: true),
                    SubProcess = table.Column<string>(nullable: true),
                    Risk = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    ControlPoint = table.Column<string>(nullable: true),
                    Frequency = table.Column<string>(nullable: true),
                    InformationProcessingObjectives = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryControls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryControls_TenantId",
                table: "LibraryControls",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryControls");
        }
    }
}
