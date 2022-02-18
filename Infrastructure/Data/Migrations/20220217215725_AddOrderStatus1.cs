using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddOrderStatus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatus1Id",
                table: "CustomerOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderStatus1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_OrderStatus1Id",
                table: "CustomerOrders",
                column: "OrderStatus1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_OrderStatus1_OrderStatus1Id",
                table: "CustomerOrders",
                column: "OrderStatus1Id",
                principalTable: "OrderStatus1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_OrderStatus1_OrderStatus1Id",
                table: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "OrderStatus1");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_OrderStatus1Id",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "OrderStatus1Id",
                table: "CustomerOrders");
        }
    }
}
