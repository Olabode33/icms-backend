using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_DepartmentRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentRatingHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    RatingDate = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<string>(nullable: true),
                    OrganizationUnitId = table.Column<long>(nullable: true),
                    RatingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRatingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRatingHistory_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentRatingHistory_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRatingHistory_OrganizationUnitId",
                table: "DepartmentRatingHistory",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRatingHistory_RatingId",
                table: "DepartmentRatingHistory",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRatingHistory_TenantId",
                table: "DepartmentRatingHistory",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentRatingHistory");
        }
    }
}
