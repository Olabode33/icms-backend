using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Loss_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LossEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DateOccured = table.Column<DateTime>(nullable: false),
                    DateDiscovered = table.Column<DateTime>(nullable: false),
                    LossType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LossCategory = table.Column<int>(nullable: false),
                    EmployeeUserId = table.Column<long>(nullable: true),
                    DepartmentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossEvents_AbpOrganizationUnits_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LossEvents_AbpUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossEvents_DepartmentId",
                table: "LossEvents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LossEvents_EmployeeUserId",
                table: "LossEvents",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LossEvents_TenantId",
                table: "LossEvents",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LossEvents");
        }
    }
}
