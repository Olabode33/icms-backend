using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Removed_Priority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "WorkingPaperReviewComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletionDate",
                table: "WorkingPaperReviewComments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompletionDate",
                table: "WorkingPaperReviewComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "WorkingPaperReviewComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
