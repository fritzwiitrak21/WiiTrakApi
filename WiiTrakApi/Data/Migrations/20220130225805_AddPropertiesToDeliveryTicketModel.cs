using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class AddPropertiesToDeliveryTicketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovedByStore",
                table: "DeliveryTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SignOffRequired",
                table: "DeliveryTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByStore",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "SignOffRequired",
                table: "DeliveryTickets");
        }
    }
}
