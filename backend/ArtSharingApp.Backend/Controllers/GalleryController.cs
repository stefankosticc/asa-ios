using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class GalleryController : Controller
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [HttpGet("galleries")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _galleryService.GetAllAsync());
    }

    [HttpGet("gallery/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var gallery = await _galleryService.GetByIdAsync(id);
        return Ok(gallery);
    }

    [HttpPost("gallery")]
    public async Task<IActionResult> Add([FromBody] GalleryRequestDTO galleryDto)
    {
        await _galleryService.AddAsync(galleryDto);
        return Ok(new {message = "Gallery added successfully."});
    }

    [HttpPut("gallery/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GalleryRequestDTO galleryDto)
    {
        await _galleryService.UpdateAsync(id, galleryDto);
        return Ok(new {message = "Gallery updated successfully."});
    }

    [HttpDelete("gallery/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _galleryService.DeleteAsync(id);
        return Ok(new { message = "Gallery deleted successfully." });;
    }
    
    [HttpGet("gallery/{id}/artworks")]
    public async Task<IActionResult> GetArtworksByGalleryId(int id)
    {
        var artworks = await _galleryService.GetArtworksByGalleryId(id);
        return Ok(artworks);
    }
    
    [HttpGet("galleries/search")]
    public async Task<IActionResult> GetGalleriesByName([FromQuery] string name)
    {
        return Ok(await _galleryService.GetGalleriesByName(name));
    }
}
