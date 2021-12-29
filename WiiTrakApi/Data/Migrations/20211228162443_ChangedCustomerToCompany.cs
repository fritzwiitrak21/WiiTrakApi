using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class ChangedCustomerToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Customers_CustomerAccountId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviders_Customers_CustomerAccountId",
                table: "ServiceProviders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerAccountId",
                table: "ServiceProviders",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceProviders_CustomerAccountId",
                table: "ServiceProviders",
                newName: "IX_ServiceProviders_CompanyId");

            migrationBuilder.RenameColumn(
                name: "CustomerAccountId",
                table: "Drivers",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_CustomerAccountId",
                table: "Drivers",
                newName: "IX_Drivers_CompanyId");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StreetAddress1 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StreetAddress2 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProfilePicUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhonePrimary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneSecondary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    CannotHaveChildren = table.Column<bool>(type: "bit", nullable: false),
                    SystemOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_SystemOwners_SystemOwnerId",
                        column: x => x.SystemOwnerId,
                        principalTable: "SystemOwners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SystemOwnerId",
                table: "Companies",
                column: "SystemOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Companies_CompanyId",
                table: "Drivers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviders_Companies_CompanyId",
                table: "ServiceProviders",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Companies_CompanyId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviders_Companies_CompanyId",
                table: "ServiceProviders");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ServiceProviders",
                newName: "CustomerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceProviders_CompanyId",
                table: "ServiceProviders",
                newName: "IX_ServiceProviders_CustomerAccountId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Drivers",
                newName: "CustomerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_CompanyId",
                table: "Drivers",
                newName: "IX_Drivers_CustomerAccountId");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhonePrimary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneSecondary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProfilePicUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StreetAddress1 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StreetAddress2 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_SystemOwners_SystemOwnerId",
                        column: x => x.SystemOwnerId,
                        principalTable: "SystemOwners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SystemOwnerId",
                table: "Customers",
                column: "SystemOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Customers_CustomerAccountId",
                table: "Drivers",
                column: "CustomerAccountId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviders_Customers_CustomerAccountId",
                table: "ServiceProviders",
                column: "CustomerAccountId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
