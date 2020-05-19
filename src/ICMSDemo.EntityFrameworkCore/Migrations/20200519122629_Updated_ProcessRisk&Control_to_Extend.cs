using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Updated_ProcessRiskControl_to_Extend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessRiskControls");

            migrationBuilder.DropTable(
                name: "ProcessRisks");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DepartmentRisks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ProcessId",
                table: "DepartmentRisks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DepartmentRiskControls",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ProcessId",
                table: "DepartmentRiskControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessRiskId",
                table: "DepartmentRiskControls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRisks_ProcessId",
                table: "DepartmentRisks",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_ProcessId",
                table: "DepartmentRiskControls",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRiskControls_ProcessRiskId",
                table: "DepartmentRiskControls",
                column: "ProcessRiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRiskControls_AbpOrganizationUnits_ProcessId",
                table: "DepartmentRiskControls",
                column: "ProcessId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRiskControls_DepartmentRisks_ProcessRiskId",
                table: "DepartmentRiskControls",
                column: "ProcessRiskId",
                principalTable: "DepartmentRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRisks_AbpOrganizationUnits_ProcessId",
                table: "DepartmentRisks",
                column: "ProcessId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRiskControls_AbpOrganizationUnits_ProcessId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRiskControls_DepartmentRisks_ProcessRiskId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRisks_AbpOrganizationUnits_ProcessId",
                table: "DepartmentRisks");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRisks_ProcessId",
                table: "DepartmentRisks");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRiskControls_ProcessId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRiskControls_ProcessRiskId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DepartmentRisks");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "DepartmentRisks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DepartmentRiskControls");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "DepartmentRiskControls");

            migrationBuilder.DropColumn(
                name: "ProcessRiskId",
                table: "DepartmentRiskControls");

            migrationBuilder.CreateTable(
                name: "ProcessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cascade = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessId = table.Column<long>(type: "bigint", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cascade = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlId = table.Column<int>(type: "int", nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessId = table.Column<long>(type: "bigint", nullable: true),
                    ProcessRiskId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
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
    }
}
