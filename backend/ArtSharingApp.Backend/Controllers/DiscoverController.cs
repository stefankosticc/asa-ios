using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class DiscoverController : AuthenticatedUserBaseController
{
    private readonly IAuctionService _auctionService;
    private readonly IFavoritesService _favoritesService;

    public DiscoverController(IAuctionService auctionService, IFavoritesService favoritesService)
    {
        _auctionService = auctionService;
        _favoritesService = favoritesService;
    }

    [HttpGet("discover")]
    public async Task<IActionResult> GetDiscoverData()
    {
        var topArtistsByLikes = await _favoritesService.GetTop10ArtistsByLikesAsync();
        var highStakeAuctions = await _auctionService.GetHighStakesAuctionsAsync(10);
        var trendingArtworks = await _favoritesService.GetTrendingArtworksAsync(20);

        return Ok(new
        {
            topArtistsByLikes,
            highStakeAuctions,
            trendingArtworks
        });
    }
}