using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class AddEnumPropForGeolocationpermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowsGeolocationFetch",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "GeolocationPermissionStatus",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeolocationPermissionStatus",
                table: "Drivers");

            migrationBuilder.AddColumn<bool>(
                name: "AllowsGeolocationFetch",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
