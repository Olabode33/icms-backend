using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Comment_And_Others_To_Project_Rating_History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CloseDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "DepartmentRatingHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_RatingId",
                table: "AbpOrganizationUnits",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_Ratings_RatingId",
                table: "AbpOrganizationUnits",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_Ratings_RatingId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_RatingId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "DepartmentRatingHistory");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "AbpOrganizationUnits");
        }
    }
}
