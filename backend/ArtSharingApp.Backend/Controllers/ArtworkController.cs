using ArtSharingApp.Backend.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ArtworkController : AuthenticatedUserBaseController
{
    private readonly IArtworkService _artworkService;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    [HttpGet("artworks")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _artworkService.GetAllAsync());
    }

    [HttpGet("artwork/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var loggedInUserId = GetLoggedInUserId();
        var artwork = await _artworkService.GetByIdAsync(id, loggedInUserId);
        return Ok(artwork);
    }

    [Authorize(Roles = "Admin, Artist")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [HttpPost("artwork")]
    public async Task<IActionResult> Add([FromForm] ArtworkRequestDTO artworkDto, [FromForm] IFormFile artworkImage)
    {
        await _artworkService.AddAsync(artworkDto, artworkImage);
        return Ok(new { message = "Artwork added successfully." });
    }

    [Authorize(Roles = "Admin, Artist")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [HttpPut("artwork/{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] ArtworkRequestDTO artworkDto,
        [FromForm] IFormFile? artworkImage)
    {
        await _artworkService.UpdateAsync(id, artworkDto, artworkImage);
        return Ok(new { message = "Artwork updated successfully." });
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpDelete("artwork/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _artworkService.DeleteAsync(id);
        return Ok(new { message = "Artwork deleted successfully." });
    }

    [HttpGet("artworks/search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        var artworks = await _artworkService.SearchByTitle(title);
        return Ok(artworks);
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpPut("artwork/{id}/change-visibility")]
    public async Task<IActionResult> ChangeVisibility(int id, [FromBody] ChangeArtworkVisibilityDTO request)
    {
        var isPrivate = request.IsPrivate;
        await _artworkService.ChangeVisibilityAsync(id, isPrivate);
        return Ok(new
            { message = $"Artwork visibility changed successfully to {(isPrivate ? "private" : "public")}." });
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpPut("artwork/{id}/put-on-sale")]
    public async Task<IActionResult> PutOnSale(int id, [FromBody] PutArtworkOnSaleDTO request)
    {
        var loggedInUserId = GetLoggedInUserId();
        await _artworkService.PutOnSaleAsync(id, loggedInUserId, request);
        return Ok(new { message = "Artwork put on sale successfully." });
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpPut("artwork/{id}/remove-from-sale")]
    public async Task<IActionResult> RemoveFromSale(int id)
    {
        var loggedInUserId = GetLoggedInUserId();
        await _artworkService.RemoveFromSaleAsync(id, loggedInUserId);
        return Ok(new { message = "Artwork removed from sale successfully." });
    }

    [HttpPut("artwork/{artworkId}/transfer/to-user/{userId}")]
    public async Task<IActionResult> TransferToUser(int artworkId, int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        await _artworkService.TransferToUserAsync(artworkId, loggedInUserId, userId);
        return Ok(new { message = "Artwork transferred successfully." });
    }

    [HttpGet("user/{userId}/artworks")]
    public async Task<IActionResult> GetUserArtworks(int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        return Ok(await _artworkService.GetUserArtworksAsync(userId, loggedInUserId));
    }

    [AllowAnonymous]
    [HttpGet("artwork/{id}/image")]
    public async Task<IActionResult> GetArtworkImage(int id)
    {
        var response = await _artworkService.GetArtworkImageAsync(id);
        return File(response.Image, response.ContentType);
    }

    [HttpPost("artwork/extract-color")]
    public async Task<IActionResult> ExtractColor([FromForm] IFormFile image)
    {
        var color = await _artworkService.ExtractColorAsync(image);
        if (color == null) return BadRequest(new { message = "Failed to extract color." });
        return Ok(color);
    }

    [HttpGet("artworks/discover")]
    public async Task<IActionResult> GetDiscoverArtworks([FromQuery] int skip, [FromQuery] int take)
    {
        var loggedInUserId = GetLoggedInUserId();
        var artworks = await _artworkService.GetDiscoverArtworksAsync(loggedInUserId, skip, take);
        return Ok(artworks);
    }
}