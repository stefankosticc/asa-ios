using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Cities_CityId",
                table: "Artworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Galleries_GalleryId",
                table: "Artworks");

            migrationBuilder.AlterColumn<int>(
                name: "GalleryId",
                table: "Artworks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Artworks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Cities_CityId",
                table: "Artworks",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Galleries_GalleryId",
                table: "Artworks",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Cities_CityId",
                table: "Artworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Galleries_GalleryId",
                table: "Artworks");

            migrationBuilder.AlterColumn<int>(
                name: "GalleryId",
                table: "Artworks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Artworks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Cities_CityId",
                table: "Artworks",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Galleries_GalleryId",
                table: "Artworks",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
