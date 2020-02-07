using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_ExceptionTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionTypeColumns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Minimum = table.Column<decimal>(nullable: true),
                    Maximum = table.Column<decimal>(nullable: true),
                    ExceptionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionTypeColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionTypeColumns_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionTypeColumns_ExceptionTypeId",
                table: "ExceptionTypeColumns",
                column: "ExceptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionTypeColumns_TenantId",
                table: "ExceptionTypeColumns",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionTypeColumns");
        }
    }
}
