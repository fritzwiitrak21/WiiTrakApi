using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class Many2ManyWithCompanyTechnicians : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SystemOwnerId",
                table: "Technicians",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Technicians",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_CompanyId",
                table: "Technicians",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Companies_CompanyId",
                table: "Technicians",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Companies_CompanyId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_CompanyId",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Technicians");

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemOwnerId",
                table: "Technicians",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
