using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class UpdateOfExceptionAttachmentField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionIncidentAttachment",
                table: "ExceptionIncidents");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionIncidentAttachments",
                table: "ExceptionIncidents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionIncidentAttachments",
                table: "ExceptionIncidents");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionIncidentAttachment",
                table: "ExceptionIncidents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
