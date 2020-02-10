using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Upateed_ExceptionIncident : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CausedById",
                table: "ExceptionIncidents",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ClosedById",
                table: "ExceptionIncidents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_CausedById",
                table: "ExceptionIncidents",
                column: "CausedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionIncidents_ClosedById",
                table: "ExceptionIncidents",
                column: "ClosedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionIncidents_AbpUsers_CausedById",
                table: "ExceptionIncidents",
                column: "CausedById",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionIncidents_AbpUsers_ClosedById",
                table: "ExceptionIncidents",
                column: "ClosedById",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionIncidents_AbpUsers_CausedById",
                table: "ExceptionIncidents");

            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionIncidents_AbpUsers_ClosedById",
                table: "ExceptionIncidents");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionIncidents_CausedById",
                table: "ExceptionIncidents");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionIncidents_ClosedById",
                table: "ExceptionIncidents");

            migrationBuilder.DropColumn(
                name: "CausedById",
                table: "ExceptionIncidents");

            migrationBuilder.DropColumn(
                name: "ClosedById",
                table: "ExceptionIncidents");
        }
    }
}
