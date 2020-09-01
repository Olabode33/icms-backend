using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_LossEventTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LossEventTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LossTypeId = table.Column<int>(nullable: true),
                    LossTypeTriggerId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AssignedTo = table.Column<long>(nullable: true),
                    DateAssigned = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossEventTasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossEventTasks_TenantId",
                table: "LossEventTasks",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LossEventTasks");
        }
    }
}
