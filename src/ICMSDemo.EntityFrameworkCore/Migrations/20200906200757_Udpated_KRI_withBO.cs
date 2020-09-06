using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Udpated_KRI_withBO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessObjectiveId",
                table: "KeyRiskIndicators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyRiskIndicators_BusinessObjectiveId",
                table: "KeyRiskIndicators",
                column: "BusinessObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyRiskIndicators_BusinessObjectives_BusinessObjectiveId",
                table: "KeyRiskIndicators",
                column: "BusinessObjectiveId",
                principalTable: "BusinessObjectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyRiskIndicators_BusinessObjectives_BusinessObjectiveId",
                table: "KeyRiskIndicators");

            migrationBuilder.DropIndex(
                name: "IX_KeyRiskIndicators_BusinessObjectiveId",
                table: "KeyRiskIndicators");

            migrationBuilder.DropColumn(
                name: "BusinessObjectiveId",
                table: "KeyRiskIndicators");
        }
    }
}
