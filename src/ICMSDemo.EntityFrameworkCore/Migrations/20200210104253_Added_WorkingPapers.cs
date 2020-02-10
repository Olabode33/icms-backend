using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Added_WorkingPapers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingPaperDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    MyProperty = table.Column<int>(nullable: false),
                    TestingAttrributeId = table.Column<int>(nullable: true),
                    Result = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    ExceptionIncidentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPaperDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingPaperDetails_ExceptionIncidents_ExceptionIncidentId",
                        column: x => x.ExceptionIncidentId,
                        principalTable: "ExceptionIncidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingPaperDetails_TestingAttrributesList_TestingAttrributeId",
                        column: x => x.TestingAttrributeId,
                        principalTable: "TestingAttrributesList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperDetails_ExceptionIncidentId",
                table: "WorkingPaperDetails",
                column: "ExceptionIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingPaperDetails_TestingAttrributeId",
                table: "WorkingPaperDetails",
                column: "TestingAttrributeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingPaperDetails");
        }
    }
}
