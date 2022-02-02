using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddPaymentOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentOptionId",
                table: "CustomerOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_PaymentOptionId",
                table: "CustomerOrders",
                column: "PaymentOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_PaymentOptions_PaymentOptionId",
                table: "CustomerOrders",
                column: "PaymentOptionId",
                principalTable: "PaymentOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_PaymentOptions_PaymentOptionId",
                table: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "PaymentOptions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_PaymentOptionId",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "PaymentOptionId",
                table: "CustomerOrders");
        }
    }
}
