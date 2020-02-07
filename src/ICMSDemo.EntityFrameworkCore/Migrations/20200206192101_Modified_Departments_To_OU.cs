using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Modified_Departments_To_OU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRisks_Departments_DepartmentId",
                table: "DepartmentRisks");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "DepartmentRisks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpOrganizationUnits",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ControlTeamId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbstract",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsControlTeam",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupervisorUserId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                column: "ControlOfficerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ControlTeamId",
                table: "AbpOrganizationUnits",
                column: "ControlTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_SupervisorUserId",
                table: "AbpOrganizationUnits",
                column: "SupervisorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId",
                table: "AbpOrganizationUnits",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ControlOfficerUserId",
                table: "AbpOrganizationUnits",
                column: "ControlOfficerUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ControlTeamId",
                table: "AbpOrganizationUnits",
                column: "ControlTeamId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_SupervisorUserId",
                table: "AbpOrganizationUnits",
                column: "SupervisorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRisks_AbpOrganizationUnits_DepartmentId",
                table: "DepartmentRisks",
                column: "DepartmentId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ControlOfficerUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ControlTeamId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_SupervisorUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRisks_AbpOrganizationUnits_DepartmentId",
                table: "DepartmentRisks");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_ControlOfficerUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_ControlTeamId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_SupervisorUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ControlOfficerUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ControlTeamId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "IsAbstract",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "IsControlTeam",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "SupervisorUserId",
                table: "AbpOrganizationUnits");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "DepartmentRisks",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlOfficerUserId = table.Column<long>(type: "bigint", nullable: true),
                    ControlTeamId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAbstract = table.Column<bool>(type: "bit", nullable: false),
                    IsControlTeam = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRisks_Departments_DepartmentId",
                table: "DepartmentRisks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
