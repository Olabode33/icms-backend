using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_KeyRiskIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyRiskIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Nature = table.Column<string>(nullable: true),
                    LowLevel = table.Column<decimal>(nullable: false),
                    LowActionType = table.Column<string>(nullable: true),
                    MediumLevel = table.Column<decimal>(nullable: false),
                    MediumActionType = table.Column<string>(nullable: true),
                    HighLevel = table.Column<decimal>(nullable: false),
                    HighActionType = table.Column<string>(nullable: true),
                    ExceptionTypeId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyRiskIndicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyRiskIndicators_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeyRiskIndicators_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyRiskIndicators_ExceptionTypeId",
                table: "KeyRiskIndicators",
                column: "ExceptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyRiskIndicators_TenantId",
                table: "KeyRiskIndicators",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyRiskIndicators_UserId",
                table: "KeyRiskIndicators",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyRiskIndicators");
        }
    }
}
