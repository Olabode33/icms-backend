using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_Escalation_To_ExceptionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionTypeEscalations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    ExceptionTypeId = table.Column<int>(nullable: false),
                    EscalationUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionTypeEscalations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionTypeEscalations_AbpUsers_EscalationUserId",
                        column: x => x.EscalationUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExceptionTypeEscalations_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionTypeEscalations_EscalationUserId",
                table: "ExceptionTypeEscalations",
                column: "EscalationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionTypeEscalations_ExceptionTypeId",
                table: "ExceptionTypeEscalations",
                column: "ExceptionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionTypeEscalations");
        }
    }
}
