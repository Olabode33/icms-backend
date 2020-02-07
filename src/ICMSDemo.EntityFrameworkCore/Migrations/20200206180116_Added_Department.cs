using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "DataLists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsAbstract = table.Column<bool>(nullable: false),
                    IsControlTeam = table.Column<bool>(nullable: false),
                    SupervisorUserId = table.Column<long>(nullable: true),
                    ControlOfficerUserId = table.Column<long>(nullable: true),
                    ControlTeamId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AbpUsers_ControlOfficerUserId",
                        column: x => x.ControlOfficerUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_AbpOrganizationUnits_ControlTeamId",
                        column: x => x.ControlTeamId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_AbpUsers_SupervisorUserId",
                        column: x => x.SupervisorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ControlOfficerUserId",
                table: "Departments",
                column: "ControlOfficerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ControlTeamId",
                table: "Departments",
                column: "ControlTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SupervisorUserId",
                table: "Departments",
                column: "SupervisorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TenantId",
                table: "Departments",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "DataLists");
        }
    }
}
