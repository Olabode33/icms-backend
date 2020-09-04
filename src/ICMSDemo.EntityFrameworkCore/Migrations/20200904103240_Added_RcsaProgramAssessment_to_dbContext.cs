using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_RcsaProgramAssessment_to_dbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RcsaProgramAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    BusinessUnitId = table.Column<long>(nullable: false),
                    DateVerified = table.Column<DateTime>(nullable: false),
                    VerificationStatus = table.Column<int>(nullable: false),
                    VerifiedByUserId = table.Column<long>(nullable: false),
                    Changes = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcsaProgramAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RcsaProgramAssessments_AbpUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RcsaProgramAssessments_VerifiedByUserId",
                table: "RcsaProgramAssessments",
                column: "VerifiedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RcsaProgramAssessments");
        }
    }
}
