using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddBirthday5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Birthdays");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus1Id",
                table: "Birthdays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Birthdays_OrderStatus1Id",
                table: "Birthdays",
                column: "OrderStatus1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Birthdays_OrderStatus1_OrderStatus1Id",
                table: "Birthdays",
                column: "OrderStatus1Id",
                principalTable: "OrderStatus1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Birthdays_OrderStatus1_OrderStatus1Id",
                table: "Birthdays");

            migrationBuilder.DropIndex(
                name: "IX_Birthdays_OrderStatus1Id",
                table: "Birthdays");

            migrationBuilder.DropColumn(
                name: "OrderStatus1Id",
                table: "Birthdays");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Birthdays",
                type: "bit",
                nullable: true);
        }
    }
}
