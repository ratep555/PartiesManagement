using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class RemoveIsAppliedOnItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAppliedOnItem",
                table: "ItemDiscounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAppliedOnItem",
                table: "ItemDiscounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
