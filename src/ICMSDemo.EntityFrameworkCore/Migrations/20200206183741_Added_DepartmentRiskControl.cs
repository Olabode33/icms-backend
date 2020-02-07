using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_DepartmentRiskControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentRiskControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Frequency = table.Column<int>(nullable: false),
                    DepartmentRiskId = table.Column<int>(nullable: true),
                    ControlId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRiskControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRiskControls_Controls_ControlId",
                        column: x => x.ControlId,
                        principalTable: "Controls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentRiskControls_DepartmentRisks_DepartmentRiskId",
                        column: x => x.DepartmentRiskId,
                        principalTable: "DepartmentRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_ControlId",
                table: "DepartmentRiskControls",
                column: "ControlId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_DepartmentRiskId",
                table: "DepartmentRiskControls",
                column: "DepartmentRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_TenantId",
                table: "DepartmentRiskControls",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentRiskControls");
        }
    }
}
