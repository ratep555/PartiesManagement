using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddBirthday2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "ServicesIncluded",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthdayNo",
                table: "Birthdays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "BirthdayPackages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "BirthdayPackages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "ServicesIncluded");

            migrationBuilder.DropColumn(
                name: "BirthdayNo",
                table: "Birthdays");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "BirthdayPackages");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "BirthdayPackages");
        }
    }
}
