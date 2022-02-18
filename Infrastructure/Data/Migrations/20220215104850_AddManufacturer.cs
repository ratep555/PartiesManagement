using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddManufacturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Manufacturer1Id",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manufacturers1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturerDiscounts",
                columns: table => new
                {
                    Manufacturer1Id = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerDiscounts", x => new { x.Manufacturer1Id, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_ManufacturerDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManufacturerDiscounts_Manufacturers1_Manufacturer1Id",
                        column: x => x.Manufacturer1Id,
                        principalTable: "Manufacturers1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Manufacturer1Id",
                table: "Items",
                column: "Manufacturer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerDiscounts_DiscountId",
                table: "ManufacturerDiscounts",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Manufacturers1_Manufacturer1Id",
                table: "Items",
                column: "Manufacturer1Id",
                principalTable: "Manufacturers1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Manufacturers1_Manufacturer1Id",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ManufacturerDiscounts");

            migrationBuilder.DropTable(
                name: "Manufacturers1");

            migrationBuilder.DropIndex(
                name: "IX_Items_Manufacturer1Id",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Manufacturer1Id",
                table: "Items");
        }
    }
}
