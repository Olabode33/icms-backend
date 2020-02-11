using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_commentz_To_Incident : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolutionComments",
                table: "ExceptionIncidents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolutionDate",
                table: "ExceptionIncidents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolutionComments",
                table: "ExceptionIncidents");

            migrationBuilder.DropColumn(
                name: "ResolutionDate",
                table: "ExceptionIncidents");
        }
    }
}
