using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentApplication.Migrations
{
    public partial class migrationn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageAppartaments",
                table: "ImageAppartaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppartamentAmeneties",
                table: "AppartamentAmeneties");

            migrationBuilder.AlterColumn<int>(
                name: "AppartamentId",
                table: "ImageAppartaments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "AppartamentId",
                table: "AppartamentAmeneties",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageAppartaments",
                table: "ImageAppartaments",
                columns: new[] { "AppartamentId", "ImageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppartamentAmeneties",
                table: "AppartamentAmeneties",
                columns: new[] { "AppartamentId", "AmenetieId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageAppartaments",
                table: "ImageAppartaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppartamentAmeneties",
                table: "AppartamentAmeneties");

            migrationBuilder.AlterColumn<int>(
                name: "AppartamentId",
                table: "ImageAppartaments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "AppartamentId",
                table: "AppartamentAmeneties",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageAppartaments",
                table: "ImageAppartaments",
                column: "AppartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppartamentAmeneties",
                table: "AppartamentAmeneties",
                column: "AppartamentId");
        }
    }
}
