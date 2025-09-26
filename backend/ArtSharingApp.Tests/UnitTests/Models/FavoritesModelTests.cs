using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class FavoritesModelTests
{
    [Fact]
    public void Constructor_SetsUserIdAndArtworkId()
    {
        var favorites = new Favorites(1, 2);
        Assert.Equal(1, favorites.UserId);
        Assert.Equal(2, favorites.ArtworkId);
    }
}