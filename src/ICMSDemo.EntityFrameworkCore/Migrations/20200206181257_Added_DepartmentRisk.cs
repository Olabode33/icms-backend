using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_DepartmentRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentRisks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    RiskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRisks_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentRisks_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRisks_DepartmentId",
                table: "DepartmentRisks",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRisks_RiskId",
                table: "DepartmentRisks",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRisks_TenantId",
                table: "DepartmentRisks",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentRisks");
        }
    }
}
