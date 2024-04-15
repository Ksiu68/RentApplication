using Microsoft.EntityFrameworkCore.Migrations;

namespace RentApplication.Migrations
{
    public partial class last : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppartamentId",
                table: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppartamentId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
