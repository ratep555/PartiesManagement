using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddAdditionalFee3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatingForBirthdayPackages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RatingForBirthdayPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    BirthdayPackageId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingForBirthdayPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingForBirthdayPackages_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RatingForBirthdayPackages_BirthdayPackages_BirthdayPackageId",
                        column: x => x.BirthdayPackageId,
                        principalTable: "BirthdayPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RatingForBirthdayPackages_ApplicationUserId",
                table: "RatingForBirthdayPackages",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingForBirthdayPackages_BirthdayPackageId",
                table: "RatingForBirthdayPackages",
                column: "BirthdayPackageId");
        }
    }
}
