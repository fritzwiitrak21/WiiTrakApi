using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class StoreCorporateOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CorporateId",
                table: "Stores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CorporateId",
                table: "Stores",
                column: "CorporateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Corporates_CorporateId",
                table: "Stores",
                column: "CorporateId",
                principalTable: "Corporates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Corporates_CorporateId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_CorporateId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CorporateId",
                table: "Stores");
        }
    }
}
