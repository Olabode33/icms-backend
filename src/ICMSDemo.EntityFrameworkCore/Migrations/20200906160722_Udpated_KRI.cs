using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Udpated_KRI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RiskId",
                table: "KeyRiskIndicators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyRiskIndicators_RiskId",
                table: "KeyRiskIndicators",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyRiskIndicators_Risks_RiskId",
                table: "KeyRiskIndicators",
                column: "RiskId",
                principalTable: "Risks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyRiskIndicators_Risks_RiskId",
                table: "KeyRiskIndicators");

            migrationBuilder.DropIndex(
                name: "IX_KeyRiskIndicators_RiskId",
                table: "KeyRiskIndicators");

            migrationBuilder.DropColumn(
                name: "RiskId",
                table: "KeyRiskIndicators");
        }
    }
}
