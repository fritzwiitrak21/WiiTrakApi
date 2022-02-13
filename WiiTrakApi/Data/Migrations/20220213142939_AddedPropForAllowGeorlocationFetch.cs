using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class AddedPropForAllowGeorlocationFetch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowsGeolocationFetch",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowsGeolocationFetch",
                table: "Drivers");
        }
    }
}
