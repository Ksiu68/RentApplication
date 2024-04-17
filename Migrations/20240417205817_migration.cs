using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentApplication.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balcony",
                table: "Appartaments");

            migrationBuilder.DropColumn(
                name: "Repair",
                table: "Appartaments");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "Appartaments");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Houses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceToMetro",
                table: "Houses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metro",
                table: "Houses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberOfFloors",
                table: "Houses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearOfConstruction",
                table: "Houses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTo3D",
                table: "Appartaments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Appartaments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countOfBedrooms",
                table: "Appartaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ameneties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ameneties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppartamentAmeneties",
                columns: table => new
                {
                    AppartamentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AmenetieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppartamentAmeneties", x => x.AppartamentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ameneties");

            migrationBuilder.DropTable(
                name: "AppartamentAmeneties");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "DistanceToMetro",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Metro",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "NumberOfFloors",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "YearOfConstruction",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "ReferenceTo3D",
                table: "Appartaments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Appartaments");

            migrationBuilder.DropColumn(
                name: "countOfBedrooms",
                table: "Appartaments");

            migrationBuilder.AddColumn<string>(
                name: "Balcony",
                table: "Appartaments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Repair",
                table: "Appartaments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wifi",
                table: "Appartaments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
