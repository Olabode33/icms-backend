using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Removed_ExceptionTypeId_From_Testing_Template_Details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestingAttrributesList_ExceptionTypes_ExceptionTypeId",
                table: "TestingAttrributesList");

            migrationBuilder.DropIndex(
                name: "IX_TestingAttrributesList_ExceptionTypeId",
                table: "TestingAttrributesList");

            migrationBuilder.DropColumn(
                name: "ExceptionTypeId",
                table: "TestingAttrributesList");

            migrationBuilder.AddColumn<int>(
                name: "ExceptionTypeId",
                table: "TestingTemplates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestingTemplates_ExceptionTypeId",
                table: "TestingTemplates",
                column: "ExceptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestingTemplates_ExceptionTypes_ExceptionTypeId",
                table: "TestingTemplates",
                column: "ExceptionTypeId",
                principalTable: "ExceptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestingTemplates_ExceptionTypes_ExceptionTypeId",
                table: "TestingTemplates");

            migrationBuilder.DropIndex(
                name: "IX_TestingTemplates_ExceptionTypeId",
                table: "TestingTemplates");

            migrationBuilder.DropColumn(
                name: "ExceptionTypeId",
                table: "TestingTemplates");

            migrationBuilder.AddColumn<int>(
                name: "ExceptionTypeId",
                table: "TestingAttrributesList",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestingAttrributesList_ExceptionTypeId",
                table: "TestingAttrributesList",
                column: "ExceptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestingAttrributesList_ExceptionTypes_ExceptionTypeId",
                table: "TestingAttrributesList",
                column: "ExceptionTypeId",
                principalTable: "ExceptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
