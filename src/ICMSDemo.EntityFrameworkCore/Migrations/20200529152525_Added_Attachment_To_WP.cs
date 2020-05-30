using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Attachment_To_WP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "WorkingPaperDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "Attachment",
                table: "WorkingPaperDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "WorkingPaperDetails");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "WorkingPaperDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
