using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class ArtworkServiceTests
{
    private readonly IArtworkService _artworkService;
    private readonly Mock<IArtworkRepository> _mockArtworkRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IFavoritesRepository> _mockFavoritesRepository;
    private readonly Mock<IMapper> _mockMapper;

    public ArtworkServiceTests()
    {
        _mockArtworkRepository = new Mock<IArtworkRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockFavoritesRepository = new Mock<IFavoritesRepository>();
        _mockMapper = new Mock<IMapper>();

        _artworkService = new ArtworkService(
            _mockArtworkRepository.Object,
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mockFavoritesRepository.Object
        );
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        // Arrange
        int nonExistentArtworkId = 999;
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(nonExistentArtworkId))!
            .ReturnsAsync((Artwork?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _artworkService.GetByIdAsync(nonExistentArtworkId, 1));
    }

    [Fact]
    public async Task AddAsync_ThrowsBadRequest_WhenArtworkDtoIsNull()
    {
        // Arrange
        IFormFile mockFormFile = new Mock<IFormFile>().Object;

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _artworkService.AddAsync(null, mockFormFile));
    }

    [Fact]
    public async Task AddAsync_ThrowsBadRequest_WhenArtworkImageIsNull()
    {
        // Arrange
        var artworkDto = new ArtworkRequestDTO
            { Title = "Test Artwork", CreatedByArtistId = 1, PostedByUserId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _artworkService.AddAsync(artworkDto, null));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        var dto = new ArtworkRequestDTO();
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.UpdateAsync(1, dto, null));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsBadRequest_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _artworkService.UpdateAsync(1, null!, null));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.DeleteAsync(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchByTitle_ThrowsBadRequest_WhenTitleIsEmpty(string title)
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _artworkService.SearchByTitle(title));
    }

    [Fact]
    public async Task ChangeVisibilityAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.ChangeVisibilityAsync(1, true));
    }

    [Fact]
    public async Task PutOnSaleAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        // Arrange
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        var dto = new PutArtworkOnSaleDTO { Price = 10, Currency = Currency.USD };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.PutOnSaleAsync(1, 1, dto));
    }

    [Fact]
    public async Task PutOnSaleAsync_ThrowsUnauthorized_WhenUserIsNotOwner()
    {
        // Arrange
        var artwork = new Artwork { Id = 1, PostedByUserId = 2 };
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artwork);
        var dto = new PutArtworkOnSaleDTO { Price = 10, Currency = Currency.USD };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _artworkService.PutOnSaleAsync(1, 1, dto));
    }

    [Fact]
    public async Task RemoveFromSaleAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.RemoveFromSaleAsync(1, 1));
    }

    [Fact]
    public async Task RemoveFromSaleAsync_ThrowsUnauthorized_WhenUserIsNotOwner()
    {
        // Arrange
        var artwork = new Artwork { Id = 1, PostedByUserId = 2 };
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artwork);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _artworkService.RemoveFromSaleAsync(1, 1));
    }

    [Fact]
    public async Task TransferToUserAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Artwork)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.TransferToUserAsync(1, 1, 2));
    }

    [Fact]
    public async Task TransferToUserAsync_ThrowsUnauthorized_WhenUserIsNotOwner()
    {
        // Arrange
        var artwork = new Artwork { Id = 1, PostedByUserId = 2 };
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artwork);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _artworkService.TransferToUserAsync(1, 1, 2));
    }

    [Fact]
    public async Task TransferToUserAsync_ThrowsNotFound_WhenToUserDoesNotExist()
    {
        // Arrange
        var artwork = new Artwork { Id = 1, PostedByUserId = 1 };
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artwork);
        _mockUserRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _artworkService.TransferToUserAsync(1, 1, 2));
    }

    [Fact]
    public async Task TransferToUserAsync_ThrowsBadRequest_WhenFromUserEqualsToUser()
    {
        // Arrange
        var artwork = new Artwork { Id = 1, PostedByUserId = 1 };
        _mockArtworkRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artwork);
        var user = new User { Id = 1 };
        _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _artworkService.TransferToUserAsync(1, 1, 1));
    }

    [Fact]
    public async Task ExtractColorAsync_ThrowsBadRequest_WhenImageIsNull()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _artworkService.ExtractColorAsync(null!));
    }

    [Fact]
    public async Task ExtractColorAsync_ThrowsBadRequest_WhenImageIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);
        
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _artworkService.ExtractColorAsync(mockFile.Object));
    }

}