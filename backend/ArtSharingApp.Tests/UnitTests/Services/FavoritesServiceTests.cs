using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using Moq;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class FavoritesServiceTests
{
    private readonly FavoritesService _favoritesService;
    private readonly Mock<IFavoritesRepository> _mockFavoritesRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IArtworkRepository> _mockArtworkRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<INotificationService> _mockNotificationService;
    
    public FavoritesServiceTests()
    {
        _mockFavoritesRepository = new Mock<IFavoritesRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockArtworkRepository = new Mock<IArtworkRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockNotificationService = new Mock<INotificationService>();

        _favoritesService = new FavoritesService(
            _mockFavoritesRepository.Object,
            _mockUserRepository.Object,
            _mockArtworkRepository.Object,
            _mockMapper.Object,
            _mockNotificationService.Object);
    }

    [Fact]
    public async Task LikeArtwork_Success()
    {
        // Arrange
        int userId = 1;
        int artworkId = 1;
        int postedByUserId = 2;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId, PostedByUserId = postedByUserId});
        _mockFavoritesRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Favorites>());
        
        // Act
        var result = await _favoritesService.LikeArtwork(userId, artworkId);

        // Assert
        Assert.True(result);
        _mockFavoritesRepository.Verify(repo => repo.AddAsync(
                It.Is<Favorites>(f => f.UserId == userId && f.ArtworkId == artworkId)),
            Times.Once);
        _mockNotificationService.Verify(service => service.CreateNotificationAsync(
            It.Is<NotificationRequestDTO>(n => n.RecipientId == postedByUserId)), Times.Once);
    }

    [Fact]
    public async Task LikeArtwork_ThrowBadRequestException_WhenArtworkAlreadyLiked()
    {
        // Arrange
        var userId = 1;
        var artworkId = 1;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId });
        _mockFavoritesRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Favorites> { new Favorites(userId, artworkId) });
        
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _favoritesService.LikeArtwork(userId, artworkId));
        _mockFavoritesRepository.Verify(repo => repo.AddAsync(It.IsAny<Favorites>()), Times.Never);
    }
    
    [Theory]
    [InlineData(-1, 1)] // User not found
    [InlineData(1, -1)] // Artwork not found
    public async Task LikeArtwork_ThrowNotFoundException_WhenUserOrArtworkNotFound(int userId, int artworkId)
    {
        // Arrange
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(userId == -1 ? null : new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(artworkId == -1 ? null : new Artwork { Id = artworkId });
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _favoritesService.LikeArtwork(userId, artworkId));
        _mockFavoritesRepository.Verify(repo => repo.AddAsync(It.IsAny<Favorites>()), Times.Never);
    }
    
    [Fact]
    public async Task DislikeArtwork_Success()
    {
        // Arrange
        int userId = 1;
        int artworkId = 1;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId });
        _mockFavoritesRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Favorites> { new Favorites(userId, artworkId) });
        
        // Act
        var result = await _favoritesService.DislikeArtwork(userId, artworkId);

        // Assert
        Assert.True(result);
        _mockFavoritesRepository.Verify(repo => repo.DeleteAsync(userId, artworkId), Times.Once);
    }
    
    [Fact]
    public async Task DislikeArtwork_ThrowBadRequestException_WhenArtworkNotLiked()
    {
        // Arrange
        int userId = 1;
        int artworkId = 1;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(new Artwork { Id = artworkId });
        _mockFavoritesRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Favorites>());
        
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _favoritesService.DislikeArtwork(userId, artworkId));
        _mockFavoritesRepository.Verify(repo => repo.DeleteAsync(userId, artworkId), Times.Never);
    }
    
    [Theory]
    [InlineData(-1, 1)] // User not found
    [InlineData(1, -1)] // Artwork not found
    public async Task DislikeArtwork_ThrowNotFoundException_WhenUserOrArtworkNotFound(int userId, int artworkId)
    {
        // Arrange
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(userId == -1 ? null : new User { Id = userId });
        _mockArtworkRepository.Setup(repo => repo.GetByIdAsync(artworkId))
            .ReturnsAsync(artworkId == -1 ? null : new Artwork { Id = artworkId });
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _favoritesService.DislikeArtwork(userId, artworkId));
        _mockFavoritesRepository.Verify(repo => repo.DeleteAsync(userId, artworkId), Times.Never);
    }
    
    [Fact]
    public async Task GetLikedArtworks_Success()
    {
        // Arrange
        int userId = 1;
        var likedArtworks = new List<Favorites>
        {
            new Favorites(userId, 1),
            new Favorites(userId, 2)
        };
        
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockFavoritesRepository.Setup(repo => repo.GetLikedArtworks(userId))
            .ReturnsAsync(likedArtworks);
        
        _mockMapper.Setup(m => m.Map<List<FavoritesDTO>>(likedArtworks))
            .Returns(new List<FavoritesDTO>
            {
                new FavoritesDTO{ArtworkId = 1},
                new FavoritesDTO{ArtworkId = 2}
            });
        
        // Act
        var result = await _favoritesService.GetLikedArtworks(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.ArtworkId == 1);
        Assert.Contains(result, a => a.ArtworkId == 2);
    }
    
    [Fact]
    public async Task GetLikedArtworks_ThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        int userId = 1;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _favoritesService.GetLikedArtworks(userId));
    }
    
    [Fact]
    public async Task GetLikedArtworks_ThrowNotFoundException_WhenNoLikedArtworksFound()
    {
        // Arrange
        int userId = 1;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockFavoritesRepository.Setup(repo => repo.GetLikedArtworks(userId))
            .ReturnsAsync(new List<Favorites>());
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _favoritesService.GetLikedArtworks(userId));
    }
}
