using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Baloon1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                table: "CustomerOrders");

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddress_CountryId",
                table: "CustomerOrders",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress_CountryId",
                table: "CustomerOrders");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
