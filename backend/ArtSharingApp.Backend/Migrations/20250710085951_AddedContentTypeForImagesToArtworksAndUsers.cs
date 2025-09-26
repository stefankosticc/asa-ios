using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedContentTypeForImagesToArtworksAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Artworks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Artworks");
        }
    }
}
