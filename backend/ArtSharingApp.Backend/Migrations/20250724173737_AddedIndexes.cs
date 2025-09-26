using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offers_AuctionId",
                table: "Offers");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_AuctionId_Amount",
                table: "Offers",
                columns: new[] { "AuctionId", "Amount" });

            migrationBuilder.CreateIndex(
                name: "IX_Auction_StartTime_EndTime",
                table: "Auctions",
                columns: new[] { "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Artwork_IsPrivate",
                table: "Artworks",
                column: "IsPrivate");

            migrationBuilder.CreateIndex(
                name: "IX_Artwork_IsPrivate_Date_CreatedByArtistId",
                table: "Artworks",
                columns: new[] { "IsPrivate", "Date", "CreatedByArtistId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offer_AuctionId_Amount",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Auction_StartTime_EndTime",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Artwork_IsPrivate",
                table: "Artworks");

            migrationBuilder.DropIndex(
                name: "IX_Artwork_IsPrivate_Date_CreatedByArtistId",
                table: "Artworks");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_AuctionId",
                table: "Offers",
                column: "AuctionId");
        }
    }
}
