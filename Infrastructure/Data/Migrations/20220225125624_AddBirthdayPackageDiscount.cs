using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddBirthdayPackageDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "BirthdayPackages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDiscountsApplied",
                table: "BirthdayPackages",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BirthdayPackageDiscounts",
                columns: table => new
                {
                    BirthdayPackageId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthdayPackageDiscounts", x => new { x.BirthdayPackageId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_BirthdayPackageDiscounts_BirthdayPackages_BirthdayPackageId",
                        column: x => x.BirthdayPackageId,
                        principalTable: "BirthdayPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BirthdayPackageDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirthdayPackageDiscounts_DiscountId",
                table: "BirthdayPackageDiscounts",
                column: "DiscountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthdayPackageDiscounts");

            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "BirthdayPackages");

            migrationBuilder.DropColumn(
                name: "HasDiscountsApplied",
                table: "BirthdayPackages");
        }
    }
}
