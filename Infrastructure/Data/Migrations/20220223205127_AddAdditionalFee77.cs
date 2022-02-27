using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddAdditionalFee77 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Birthdays_Locations_LocationId",
                table: "Birthdays");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Birthdays",
                newName: "Location1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Birthdays_LocationId",
                table: "Birthdays",
                newName: "IX_Birthdays_Location1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Birthdays_Locations_Location1Id",
                table: "Birthdays",
                column: "Location1Id",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Birthdays_Locations_Location1Id",
                table: "Birthdays");

            migrationBuilder.RenameColumn(
                name: "Location1Id",
                table: "Birthdays",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Birthdays_Location1Id",
                table: "Birthdays",
                newName: "IX_Birthdays_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Birthdays_Locations_LocationId",
                table: "Birthdays",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
