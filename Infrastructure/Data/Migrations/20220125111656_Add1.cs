using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.migrations
{
    public partial class Add1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryShippingOptions");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShippingOptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShippingOptions");

            migrationBuilder.CreateTable(
                name: "CountryShippingOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryShippingOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryShippingOptions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryShippingOptions_ShippingOptions_ShippingOptionId",
                        column: x => x.ShippingOptionId,
                        principalTable: "ShippingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryShippingOptions_CountryId",
                table: "CountryShippingOptions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryShippingOptions_ShippingOptionId",
                table: "CountryShippingOptions",
                column: "ShippingOptionId");
        }
    }
}
