using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using Moq;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class AuctionServiceTests
{
    private readonly IAuctionService _auctionService;
    private readonly Mock<IAuctionRepository> _mockAuctionRepository;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IArtworkRepository> _mockArtworkRepository;
    private readonly Mock<IMapper> _mockMapper;

    public AuctionServiceTests()
    {
        _mockAuctionRepository = new Mock<IAuctionRepository>();
        _mockOfferRepository = new Mock<IOfferRepository>();
        _mockArtworkRepository = new Mock<IArtworkRepository>();
        _mockMapper = new Mock<IMapper>();

        _auctionService = new AuctionService(
            _mockAuctionRepository.Object,
            _mockArtworkRepository.Object,
            _mockOfferRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task StartAuctionAsync_Success()
    {
        // Arrange
        int artworkId = 1;
        int userId = 1;
        var request = new AuctionStartDTO
        {
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = userId });
        _mockAuctionRepository.Setup(repo => repo.IsAuctionScheduledAsync(
                artworkId, request.StartTime, request.EndTime))
            .ReturnsAsync(false);
        _mockAuctionRepository.Setup(repo => repo.HasFutureAuctionScheduledAsync(
                artworkId, It.IsAny<DateTime>()))
            .ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<Auction>(request))
            .Returns(new Auction
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ArtworkId = artworkId
            });

        // Act
        await _auctionService.StartAuctionAsync(artworkId, userId, request);

        // Assert
        _mockAuctionRepository.Verify(repo => repo.AddAsync(It.IsAny<Auction>()), Times.Once);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task StartAuctionAsync_ThrowUnauthorizedAccessException_WhenUserIsNotOwner()
    {
        // Arrange
        int artworkId = 1;
        int userId = 1;
        var request = new AuctionStartDTO
        {
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = 2 });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _auctionService.StartAuctionAsync(artworkId, userId, request));
        _mockAuctionRepository.Verify(repo => repo.AddAsync(It.IsAny<Auction>()), Times.Never);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Theory]
    [InlineData(-1, 1)] // Start time in the past
    [InlineData(1, -1)] // End time before start time
    [InlineData(0, 0)] // End time equal to start time
    public async Task StartAuctionAsync_ThrowBadRequestException_WhenInvalidTime(int startTimeOffset, int endTimeOffset)
    {
        // Arrange
        int artworkId = 1;
        int userId = 1;
        var request = new AuctionStartDTO
        {
            StartTime = DateTime.UtcNow.AddHours(startTimeOffset).AddMinutes(-1),
            EndTime = DateTime.UtcNow.AddHours(endTimeOffset)
        };

        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = userId });

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _auctionService.StartAuctionAsync(artworkId, userId, request));
        _mockAuctionRepository.Verify(repo => repo.AddAsync(It.IsAny<Auction>()), Times.Never);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task StartAuctionAsync_ThrowBadRequestException_WhenFutureAuctionAlreadyExists()
    {
        // Arrange
        int artworkId = 1;
        int userId = 1;
        var request = new AuctionStartDTO
        {
            StartTime = DateTime.UtcNow.AddHours(3),
            EndTime = DateTime.UtcNow.AddHours(4)
        };

        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = userId });
        _mockAuctionRepository.Setup(repo =>
                repo.HasFutureAuctionScheduledAsync(artworkId, It.IsAny<DateTime>()))
            .ReturnsAsync(true);
        _mockAuctionRepository.Setup(repo =>
                repo.IsAuctionScheduledAsync(artworkId, request.StartTime, request.EndTime))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _auctionService.StartAuctionAsync(artworkId, userId, request));

        _mockAuctionRepository.Verify(repo => repo.AddAsync(It.IsAny<Auction>()), Times.Never);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task StartAuctionAsync_ThrowBadRequestException_WhenAuctionAlreadyScheduled()
    {
        // Arrange
        int artworkId = 1;
        int userId = 1;
        var request = new AuctionStartDTO
        {
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = userId });
        _mockAuctionRepository.Setup(repo => repo.IsAuctionScheduledAsync(
                artworkId, request.StartTime, request.EndTime))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _auctionService.StartAuctionAsync(artworkId, userId, request));
        _mockAuctionRepository.Verify(repo => repo.AddAsync(It.IsAny<Auction>()), Times.Never);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task MakeAnOfferAsync_Success()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new OfferRequestDTO
        {
            Amount = 100
        };

        _mockAuctionRepository.Setup(repo =>
                repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                Artwork = new Artwork { Id = 1, PostedByUserId = 2 }
            });
        _mockOfferRepository.Setup(repo => repo.GetMaxOfferAmountAsync(auctionId))
            .ReturnsAsync(request.Amount - 10);
        _mockMapper.Setup(m => m.Map<Offer>(request))
            .Returns(new Offer
            {
                Amount = request.Amount,
                AuctionId = auctionId,
                UserId = userId
            });

        // Act
        await _auctionService.MakeAnOfferAsync(auctionId, userId, request);

        // Assert
        _mockOfferRepository.Verify(repo => repo.AddAsync(It.IsAny<Offer>()), Times.Once);
        _mockOfferRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1, 2)] // Auction not yet started
    [InlineData(-2, -1)] // Auction ended
    public async Task MakeAnOfferAsync_ThrowBadRequestException_WhenAuctionNotActive(int startTimeOffset,
        int endTimeOffset)
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new OfferRequestDTO
        {
            Amount = 100
        };

        _mockAuctionRepository.Setup(repo =>
                repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                StartTime = DateTime.UtcNow.AddHours(startTimeOffset),
                EndTime = DateTime.UtcNow.AddHours(endTimeOffset),
                Artwork = new Artwork { Id = 1, PostedByUserId = 2 }
            });

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _auctionService.MakeAnOfferAsync(auctionId, userId, request));
        _mockOfferRepository.Verify(repo => repo.AddAsync(It.IsAny<Offer>()), Times.Never);
        _mockOfferRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Theory]
    [InlineData(100, 80, 0)] // Offer less than starting price
    [InlineData(80, 100, 110)] // Offer less than max offer
    [InlineData(80, 100, 100)] // Offer equal to max offer
    public async Task MakeAnOfferAsync_ThrowBadRequestException_WhenOfferNotValid(decimal startingPrice,
        decimal offerAmount, decimal maxOffer)
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new OfferRequestDTO
        {
            Amount = offerAmount
        };

        _mockAuctionRepository.Setup(repo =>
                repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                StartingPrice = startingPrice,
                Artwork = new Artwork { Id = 1, PostedByUserId = 2 }
            });
        _mockOfferRepository.Setup(repo => repo.GetMaxOfferAmountAsync(auctionId))
            .ReturnsAsync(maxOffer);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _auctionService.MakeAnOfferAsync(auctionId, userId, request));
        _mockOfferRepository.Verify(repo => repo.AddAsync(It.IsAny<Offer>()), Times.Never);
        _mockOfferRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task MakeAnOfferAsync_ThrowUnauthorizedAccessException_WhenUserPutsOfferOnHisOwnAuction()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new OfferRequestDTO
        {
            Amount = 100
        };

        _mockAuctionRepository.Setup(repo =>
                repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                Artwork = new Artwork { Id = 1, PostedByUserId = userId }
            });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _auctionService.MakeAnOfferAsync(auctionId, userId, request));
        _mockOfferRepository.Verify(repo => repo.AddAsync(It.IsAny<Offer>()), Times.Never);
        _mockOfferRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Theory]
    [InlineData(0, 0, 100)] // No offers, current price = starting price
    [InlineData(2, 150, 150)] // Max offer present, current price = max offer
    public async Task GetActiveAuctionAsync_Success(int offerCount, decimal maxOffer, decimal expectedCurrentPrice)
    {
        // Arrange
        int artworkId = 1;
        int auctionId = 123;
        decimal startingPrice = 100;
        var now = DateTime.UtcNow;

        var auction = new Auction
        {
            Id = auctionId,
            StartTime = now.AddMinutes(-15),
            EndTime = now.AddMinutes(15),
            StartingPrice = startingPrice,
            Currency = Currency.GBP
        };

        _mockAuctionRepository
            .Setup(repo => repo.GetActiveAuctionByArtworkIdAsync(artworkId, It.IsAny<DateTime>()))
            .ReturnsAsync(auction);
        _mockOfferRepository
            .Setup(repo => repo.GetMaxOfferAmountAsync(auctionId))
            .ReturnsAsync(maxOffer);
        _mockOfferRepository
            .Setup(repo => repo.GetOfferCountByAuctionIdAsync(auctionId))
            .ReturnsAsync(offerCount);

        // Act
        var result = await _auctionService.GetActiveAuctionAsync(artworkId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(auctionId, result.Id);
        Assert.Equal(auction.StartTime, result.StartTime);
        Assert.Equal(auction.EndTime, result.EndTime);
        Assert.Equal(auction.Currency, result.Currency);
        Assert.Equal(offerCount, result.OfferCount);
        Assert.Equal(expectedCurrentPrice, result.CurrentPrice);
    }

    [Fact]
    public async Task GetActiveAuctionAsync_ReturnsNull_WhenNoActiveAuctionExists()
    {
        // Arrange
        int artworkId = 1;

        _mockAuctionRepository
            .Setup(repo => repo.GetActiveAuctionByArtworkIdAsync(artworkId, It.IsAny<DateTime>()))
            .ReturnsAsync((Auction?)null);

        // Act
        var result = await _auctionService.GetActiveAuctionAsync(artworkId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetOffersAsync_Success()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var offers = new List<Offer>
        {
            new Offer { Id = 1, Amount = 100, UserId = userId },
            new Offer { Id = 2, Amount = 150, UserId = userId + 1 }
        };

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction { Id = auctionId, Artwork = new Artwork { PostedByUserId = userId } });
        _mockOfferRepository
            .Setup(repo => repo.GetOffersByAuctionIdAsync(auctionId))
            .ReturnsAsync(offers);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<OfferResponseDTO>>(offers))
            .Returns(offers.Select(o => new OfferResponseDTO
            {
                Id = o.Id,
                Amount = o.Amount,
                UserId = o.UserId
            }));

        // Act
        var result = await _auctionService.GetOffersAsync(auctionId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, offer => Assert.IsType<OfferResponseDTO>(offer));
        _mockOfferRepository.Verify(repo => repo.GetOffersByAuctionIdAsync(auctionId), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<OfferResponseDTO>>(offers), Times.Once);
    }
    
    [Fact]
    public async Task GetOffersAsync_AuctionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))!
            .ReturnsAsync((Auction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _auctionService.GetOffersAsync(auctionId, userId));
        _mockOfferRepository.Verify(repo => repo.GetOffersByAuctionIdAsync(auctionId), Times.Never);
    }
    
    [Fact]
    public async Task GetOffersAsync_UserIsNotOwner_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                Artwork = new Artwork { PostedByUserId = userId + 1 }
            });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _auctionService.GetOffersAsync(auctionId, userId));
        _mockOfferRepository.Verify(repo => repo.GetOffersByAuctionIdAsync(auctionId), Times.Never);
    }
    
    [Fact]
    public async Task UpdateAuctionEndTimeAsync_AuctionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new AuctionUpdateEndDTO { EndTime = DateTime.UtcNow.AddDays(1) };

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))!
            .ReturnsAsync((Auction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => 
            _auctionService.UpdateAuctionEndTimeAsync(auctionId, userId, request));
    }
    
    [Fact]
    public async Task UpdateAuctionEndTimeAsync_UserIsNotOwner_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new AuctionUpdateEndDTO { EndTime = DateTime.UtcNow.AddDays(1) };

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(new Auction
            {
                Id = auctionId,
                Artwork = new Artwork { PostedByUserId = userId + 1 }
            });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _auctionService.UpdateAuctionEndTimeAsync(auctionId, userId, request));
    }
    
    [Fact]
    public async Task UpdateAuctionEndTimeAsync_Success()
    {
        // Arrange
        int auctionId = 1;
        int userId = 1;
        var request = new AuctionUpdateEndDTO { EndTime = DateTime.UtcNow.AddDays(1) };

        var auction = new Auction
        {
            Id = auctionId,
            Artwork = new Artwork { PostedByUserId = userId },
            EndTime = DateTime.UtcNow.AddHours(1)
        };

        _mockAuctionRepository
            .Setup(repo => repo.GetByIdAsync(auctionId, ac => ac.Artwork))
            .ReturnsAsync(auction);

        // Act
        await _auctionService.UpdateAuctionEndTimeAsync(auctionId, userId, request);

        // Assert
        Assert.Equal(request.EndTime, auction.EndTime);
        _mockAuctionRepository.Verify(repo => repo.UpdateEndTime(auction), Times.Once);
        _mockAuctionRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}