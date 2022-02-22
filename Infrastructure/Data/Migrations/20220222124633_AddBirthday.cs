using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddBirthday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirthdayPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthdayPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicesIncluded",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descriptiom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesIncluded", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Birthdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthdayGirlBoyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    BirthdayPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birthdays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Birthdays_BirthdayPackages_BirthdayPackageId",
                        column: x => x.BirthdayPackageId,
                        principalTable: "BirthdayPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Birthdays_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BirthdayPackageServices",
                columns: table => new
                {
                    BirthdayPackageId = table.Column<int>(type: "int", nullable: false),
                    ServiceIncludedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthdayPackageServices", x => new { x.BirthdayPackageId, x.ServiceIncludedId });
                    table.ForeignKey(
                        name: "FK_BirthdayPackageServices_BirthdayPackages_BirthdayPackageId",
                        column: x => x.BirthdayPackageId,
                        principalTable: "BirthdayPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BirthdayPackageServices_ServicesIncluded_ServiceIncludedId",
                        column: x => x.ServiceIncludedId,
                        principalTable: "ServicesIncluded",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirthdayPackageServices_ServiceIncludedId",
                table: "BirthdayPackageServices",
                column: "ServiceIncludedId");

            migrationBuilder.CreateIndex(
                name: "IX_Birthdays_BirthdayPackageId",
                table: "Birthdays",
                column: "BirthdayPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Birthdays_LocationId",
                table: "Birthdays",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthdayPackageServices");

            migrationBuilder.DropTable(
                name: "Birthdays");

            migrationBuilder.DropTable(
                name: "ServicesIncluded");

            migrationBuilder.DropTable(
                name: "BirthdayPackages");
        }
    }
}
