using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddPaymentStatus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus1Id",
                table: "CustomerOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentStatuses1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_PaymentStatus1Id",
                table: "CustomerOrders",
                column: "PaymentStatus1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_PaymentStatuses1_PaymentStatus1Id",
                table: "CustomerOrders",
                column: "PaymentStatus1Id",
                principalTable: "PaymentStatuses1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_PaymentStatuses1_PaymentStatus1Id",
                table: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "PaymentStatuses1");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_PaymentStatus1Id",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus1Id",
                table: "CustomerOrders");
        }
    }
}
