using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_LossTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LossTypeColumns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ColumnName = table.Column<string>(nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    LossType = table.Column<int>(nullable: false),
                    Minimum = table.Column<double>(nullable: true),
                    Maximum = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossTypeColumns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossTypeColumns_TenantId",
                table: "LossTypeColumns",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LossTypeColumns");
        }
    }
}
