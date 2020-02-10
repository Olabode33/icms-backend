using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Testing_Templates_Attr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TestingTemplates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TestingAttrributesList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TestAttribute = table.Column<string>(nullable: true),
                    TestingTemplateId = table.Column<int>(nullable: true),
                    ExceptionTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingAttrributesList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingAttrributesList_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingAttrributesList_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestingAttrributesList_ExceptionTypeId",
                table: "TestingAttrributesList",
                column: "ExceptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingAttrributesList_TestingTemplateId",
                table: "TestingAttrributesList",
                column: "TestingTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestingAttrributesList");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TestingTemplates");
        }
    }
}
