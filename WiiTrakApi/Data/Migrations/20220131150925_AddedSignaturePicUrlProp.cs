using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class AddedSignaturePicUrlProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignaturePicUrl",
                table: "DeliveryTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignaturePicUrl",
                table: "DeliveryTickets");
        }
    }
}
