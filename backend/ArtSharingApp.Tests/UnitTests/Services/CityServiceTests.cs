using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using Moq;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class CityServiceTests
{
    private readonly ICityService _cityService;
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IMapper> _mockMapper;

    public CityServiceTests()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockMapper = new Mock<IMapper>();
        _cityService = new CityService(_mockCityRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddAsync_ThrowsBadRequestException_WhenCityDtoIsNull()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.AddAsync(null));
    }

    [Theory]
    [InlineData(null, "Country")]
    [InlineData("City", null)]
    [InlineData("", "Country")]
    [InlineData("City", "")]
    [InlineData("   ", "Country")]
    [InlineData("City", "   ")]
    public async Task AddAsync_ThrowsBadRequestException_WhenCityDtoIsInvalid(string name, string country)
    {
        var cityDto = new CityRequestDTO { Name = name, Country = country };
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.AddAsync(cityDto));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsBadRequestException_WhenCityDtoIsNull()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.UpdateAsync(1, null));
    }

    [Theory]
    [InlineData(null, "Country")]
    [InlineData("City", null)]
    [InlineData("", "Country")]
    [InlineData("City", "")]
    [InlineData("   ", "Country")]
    [InlineData("City", "   ")]
    public async Task UpdateAsync_ThrowsBadRequestException_WhenCityDtoIsInvalid(string name, string country)
    {
        var cityDto = new CityRequestDTO { Name = name, Country = country };
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.UpdateAsync(1, cityDto));
    }

    [Fact]
    public async Task GetCitiesByName_ThrowsBadRequestException_WhenNameIsNullOrEmpty()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.GetCitiesByName(null));
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.GetCitiesByName(string.Empty));
        await Assert.ThrowsAsync<BadRequestException>(() => _cityService.GetCitiesByName("   "));
    }
}