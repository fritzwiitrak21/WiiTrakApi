using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiiTrakApi.Migrations
{
    public partial class UpdatedDeliverTicketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "DroppedOffAt",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "DeliveryTickets");

            migrationBuilder.RenameColumn(
                name: "PickedUpAt",
                table: "DeliveryTickets",
                newName: "DeliveredAt");

            migrationBuilder.RenameColumn(
                name: "Condition",
                table: "DeliveryTickets",
                newName: "NumberOfCarts");

            migrationBuilder.AddColumn<long>(
                name: "DeliveryTicketNumber",
                table: "DeliveryTickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Grid",
                table: "DeliveryTickets",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PicUrl",
                table: "DeliveryTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryTicketId",
                table: "CartHistory",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTicketNumber",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "Grid",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "PicUrl",
                table: "DeliveryTickets");

            migrationBuilder.DropColumn(
                name: "DeliveryTicketId",
                table: "CartHistory");

            migrationBuilder.RenameColumn(
                name: "NumberOfCarts",
                table: "DeliveryTickets",
                newName: "Condition");

            migrationBuilder.RenameColumn(
                name: "DeliveredAt",
                table: "DeliveryTickets",
                newName: "PickedUpAt");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "DeliveryTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DroppedOffAt",
                table: "DeliveryTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "DeliveryTickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "DeliveryTickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
