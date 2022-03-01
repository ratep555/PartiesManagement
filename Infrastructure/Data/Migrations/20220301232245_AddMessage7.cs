using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddMessage7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Locations_Location1Id",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_Location1Id",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Location1Id",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Location1Id",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Location1Id",
                table: "Messages",
                column: "Location1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Locations_Location1Id",
                table: "Messages",
                column: "Location1Id",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
