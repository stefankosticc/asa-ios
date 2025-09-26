using ArtSharingApp.Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using ArtSharingApp.Backend.Controllers.Common;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FavoritesController : AuthenticatedUserBaseController
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [HttpPost("artwork/{artworkId}/like")]
    public async Task<IActionResult> LikeArtwork(int artworkId)
    {
        var userId = GetLoggedInUserId();
        var liked = await _favoritesService.LikeArtwork(userId, artworkId);
        if (liked)
        {
            return Ok(new { message = "Artwork liked successfully." });
        }
        return BadRequest(new { message = "Failed to like artwork." });
    }

    [HttpDelete("artwork/{artworkId}/dislike")]
    public async Task<IActionResult> DislikeArtwork(int artworkId)
    {
        var userId = GetLoggedInUserId();
        var disliked = await _favoritesService.DislikeArtwork(userId, artworkId);
        if (disliked)
        {
            return Ok(new { message = "Artwork disliked successfully." });
        }
        return BadRequest(new { message = "Failed to dislike artwork." });
    }

    [HttpGet("user/{userId}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks(int userId)
    {
        var likedArtworks = await _favoritesService.GetLikedArtworks(userId);
        if (likedArtworks != null)
        {
            return Ok(likedArtworks);
        }
        throw new NotFoundException("No liked artworks found.");
    }
    
    [HttpGet("top-artists")]
    public async Task<IActionResult> GetTop10ArtistsByLikes()
    {
        var users = await _favoritesService.GetTop10ArtistsByLikesAsync();
        return Ok(users);
    }
    
    [HttpGet("trending-artworks")]
    public async Task<IActionResult> GetTrendingArtworks([FromQuery] int count)
    {
        var artworks = await _favoritesService.GetTrendingArtworksAsync(count);
        return Ok(artworks);
    }
}
