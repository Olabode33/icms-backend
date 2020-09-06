using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICMSDemo.Migrations
{
    public partial class Udpate_RCSA_Assessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcsaProgramAssessments_AbpUsers_VerifiedByUserId",
                table: "RcsaProgramAssessments");

            migrationBuilder.AlterColumn<long>(
                name: "VerifiedByUserId",
                table: "RcsaProgramAssessments",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateVerified",
                table: "RcsaProgramAssessments",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_RcsaProgramAssessments_AbpUsers_VerifiedByUserId",
                table: "RcsaProgramAssessments",
                column: "VerifiedByUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcsaProgramAssessments_AbpUsers_VerifiedByUserId",
                table: "RcsaProgramAssessments");

            migrationBuilder.AlterColumn<long>(
                name: "VerifiedByUserId",
                table: "RcsaProgramAssessments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateVerified",
                table: "RcsaProgramAssessments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RcsaProgramAssessments_AbpUsers_VerifiedByUserId",
                table: "RcsaProgramAssessments",
                column: "VerifiedByUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
