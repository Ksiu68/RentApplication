using Microsoft.EntityFrameworkCore.Migrations;

namespace RentApplication.Migrations
{
    public partial class lastmigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Appartaments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Appartaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
