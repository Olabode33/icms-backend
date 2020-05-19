using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_process_risk__control : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Cascade = table.Column<bool>(nullable: false),
                    ProcessId = table.Column<long>(nullable: false),
                    RiskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessRisks_AbpOrganizationUnits_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessRisks_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessRiskControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Frequency = table.Column<int>(nullable: false),
                    Cascade = table.Column<bool>(nullable: false),
                    ProcessRiskId = table.Column<int>(nullable: true),
                    ProcessId = table.Column<long>(nullable: true),
                    ControlId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRiskControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessRiskControls_Controls_ControlId",
                        column: x => x.ControlId,
                        principalTable: "Controls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRiskControls_AbpOrganizationUnits_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRiskControls_ProcessRisks_ProcessRiskId",
                        column: x => x.ProcessRiskId,
                        principalTable: "ProcessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRiskControls_ControlId",
                table: "ProcessRiskControls",
                column: "ControlId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRiskControls_ProcessId",
                table: "ProcessRiskControls",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRiskControls_ProcessRiskId",
                table: "ProcessRiskControls",
                column: "ProcessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRiskControls_TenantId",
                table: "ProcessRiskControls",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRisks_ProcessId",
                table: "ProcessRisks",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRisks_RiskId",
                table: "ProcessRisks",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRisks_TenantId",
                table: "ProcessRisks",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessRiskControls");

            migrationBuilder.DropTable(
                name: "ProcessRisks");
        }
    }
}
