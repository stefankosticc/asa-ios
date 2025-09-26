using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class AuctionController : AuthenticatedUserBaseController
{
    private readonly IAuctionService _auctionService;
    
    public AuctionController(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }
    
    [Authorize(Roles = "Artist, Admin")]
    [HttpPost("artwork/{artworkId}/auction/start")]
    public async Task<IActionResult> StartAuction(int artworkId, [FromBody] AuctionStartDTO request)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.StartAuctionAsync(artworkId, userId, request);
        return Ok(new { message = "Auction started successfully." });
    }
    
    [HttpPost("auction/{auctionId}/make-an-offer")]
    public async Task<IActionResult> MakeAnOffer(int auctionId, [FromBody] OfferRequestDTO request)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.MakeAnOfferAsync(auctionId, userId, request);
        return Ok(new { message = "Offer submitted." });
    }
    
    [Authorize(Roles = "Artist, Admin")]
    [HttpGet("auction/{auctionId}/offers")]
    public async Task<IActionResult> GetOffers(int auctionId)
    {
        var userId = GetLoggedInUserId();
        var offers = await _auctionService.GetOffersAsync(auctionId, userId);
        return Ok(offers);
    }
    
    [HttpGet("auction/{auctionId}/offers/max")]
    public async Task<IActionResult> GetMaxOffer(int auctionId)
    {
        var maxOffer = await _auctionService.GetMaxOfferAsync(auctionId);
        return Ok(maxOffer);
    }
    
    [Authorize(Roles = "Artist, Admin")]
    [HttpPut("offer/{offerId}/accept")]
    public async Task<IActionResult> AcceptOffer(int offerId)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.AcceptOfferAsync(offerId, userId);
        return Ok(new { message = "Offer accepted." });
    }
    
    [HttpPut("offer/{offerId}/reject")]
    public async Task<IActionResult> RejectOffer(int offerId)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.RejectOfferAsync(offerId, userId);
        return Ok(new { message = "Offer rejected." });
    }
    
    [HttpPut("offer/{offerId}/withdraw")]
    public async Task<IActionResult> WithdrawOffer(int offerId)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.WithdrawOfferAsync(offerId, userId);
        return Ok(new { message = "Offer withdrawn." });
    }
    
    [HttpGet("artwork/{artworkId}/auction/active")]
    public async Task<IActionResult> GetActiveAuction(int artworkId)
    {
        var auction = await _auctionService.GetActiveAuctionAsync(artworkId);
        return Ok(auction);
    }
    
    [Authorize(Roles = "Artist, Admin")]
    [HttpPut("auction/{auctionId}")]
    public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] AuctionUpdateEndDTO request)
    {
        var userId = GetLoggedInUserId();
        await _auctionService.UpdateAuctionEndTimeAsync(auctionId, userId, request);
        return Ok(new { message = "Auction updated successfully." });
    }
    
    [HttpGet("auctions/high-stakes")]
    public async Task<IActionResult> GetHighStakesAuctions([FromQuery]int count)
    {
        var auctions = await _auctionService.GetHighStakesAuctionsAsync(count);
        return Ok(auctions);
    }
}