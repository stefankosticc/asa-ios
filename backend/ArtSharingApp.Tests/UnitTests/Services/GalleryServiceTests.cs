using System.Linq.Expressions;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using AutoMapper;
using Moq;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class GalleryServiceTests
{
    private readonly GalleryService _galleryService;
    private readonly Mock<IGalleryRepository> _mockGalleryRepository;
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IMapper> _mockMapper;

    public GalleryServiceTests()
    {
        _mockGalleryRepository = new Mock<IGalleryRepository>();
        _mockCityRepository = new Mock<ICityRepository>();
        _mockMapper = new Mock<IMapper>();
        _galleryService = new GalleryService(
            _mockGalleryRepository.Object,
            _mockMapper.Object,
            _mockCityRepository.Object);
    }

    [Fact]
    public async Task GetArtworksByGalleryId_Success()
    {
        // Arrange
        int galleryId = 1;
        var artworks = new List<Artwork>
        {
            new Artwork { Id = 10, Title = "Artwork 1" },
            new Artwork { Id = 20, Title = "Artwork 2" }
        };
        var gallery = new Gallery
        {
            Id = galleryId,
            Name = "Test Gallery",
            Artworks = artworks
        };

        _mockGalleryRepository
            .Setup(repo => repo.GetByIdAsync(galleryId, It.IsAny<Expression<Func<Gallery, object>>[]>()))
            .ReturnsAsync(gallery);

        _mockMapper.Setup(m => m.Map<IEnumerable<ArtworkResponseDTO>>(artworks))
            .Returns(new List<ArtworkResponseDTO>
            {
                new ArtworkResponseDTO { Id = 10, Title = "Artwork 1" },
                new ArtworkResponseDTO { Id = 20, Title = "Artwork 2" }
            });

        // Act
        var result = await _galleryService.GetArtworksByGalleryId(galleryId);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, a => a.Id == 10);
        Assert.Contains(resultList, a => a.Id == 20);
    }

    [Fact]
    public async Task GetArtworksByGalleryId_ThrowNotFoundException_WhenGalleryNotFound()
    {
        // Arrange
        int galleryId = 1;

        _mockGalleryRepository
            .Setup(repo => repo.GetByIdAsync(galleryId, It.IsAny<Expression<Func<Gallery, object>>[]>()))
            .ReturnsAsync((Gallery)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _galleryService.GetArtworksByGalleryId(galleryId));
    }
}