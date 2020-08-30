using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class AddedLossTypeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LossType",
                table: "LossTypeColumns");

            migrationBuilder.AddColumn<int>(
                name: "LossTypeId",
                table: "LossTypeColumns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LossTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LossTypeTriggers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Frequency = table.Column<int>(nullable: false),
                    Staff = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    DataSource = table.Column<string>(nullable: true),
                    LossTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossTypeTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossTypeTriggers_LossTypes_LossTypeId",
                        column: x => x.LossTypeId,
                        principalTable: "LossTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossTypeTriggers_LossTypeId",
                table: "LossTypeTriggers",
                column: "LossTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LossTypeTriggers");

            migrationBuilder.DropTable(
                name: "LossTypes");

            migrationBuilder.DropColumn(
                name: "LossTypeId",
                table: "LossTypeColumns");

            migrationBuilder.AddColumn<int>(
                name: "LossType",
                table: "LossTypeColumns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
