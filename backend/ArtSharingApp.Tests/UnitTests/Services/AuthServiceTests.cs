using System.Security.Claims;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class AuthServiceTests
{
    private readonly IAuthService _authService;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<RoleManager<Role>> _mockRoleManager;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public AuthServiceTests()
    {
        _mockUserManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);
        _mockRoleManager = new Mock<RoleManager<Role>>(
            new Mock<IRoleStore<Role>>().Object,
            null, null, null, null);

        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Jwt:Token"]).Returns(
            "2840e2b449c5f7b0de74694d1ddd1ae85b45f96e1110bc95d51b398a91e24895140cd5608ed2745179350c9963ce29f054aca1b0a6842c3bc6b4af45b8c80aea0ceb447d2e64bbfe7b0114fbc076dbe57ba61b0199c72c00bee7de3aaf1bab257b7cb5e9905fe14da46cc7b9b84e6eb75cbf593e45766ffc007e774164e59548476cb51e9dfadc53b42866283785d1975bb76eb4ef575dcca57357276f6ebb2deca41160d11e45a96a956961664c9bf28868f85bab426187599772d647082e81a61b0e39064cc451fd8fdc89c61979af2666bbaa8fb4b67ab2568a8ff31d29107f43ae259e2a779c567d0e74b69b1e68d662183794e6ce3bdb5ea02a1fc6d9f0");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("test_issuer");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("test_audience");

        _authService = new AuthService(
            _mockUserManager.Object,
            _mockRoleManager.Object,
            _mockConfiguration.Object);
    }

    [Fact]
    public async Task Register_Success()
    {
        // Arrange
        var request = new UserRegisterDTO()
        {
            UserName = "testuser",
            Email = "test@gmail.com",
            Name = "Test User",
            Password = "Test.1234"
        };

        var role = new Role() { Id = 1, Name = "Artist" };

        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync((User)null);
        _mockRoleManager.Setup(rm => rm.FindByNameAsync("Artist"))
            .ReturnsAsync(role);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), role.Name))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _authService.Register(request);

        // Assert
        _mockUserManager.Verify(um => um.CreateAsync(
            It.Is<User>(
                u => u.UserName == request.UserName &&
                     u.Email == request.Email &&
                     u.Name == request.Name &&
                     u.RoleId == role.Id
            ), request.Password), Times.Once);
        _mockUserManager.Verify(um => um.AddToRoleAsync(
            It.Is<User>(
                u => u.UserName == request.UserName &&
                     u.Email == request.Email
            ), role.Name), Times.Once);
    }

    [Fact]
    public async Task Register_UserAlreadyExists_ThrowsBadRequestException()
    {
        // Arrange
        var request = new UserRegisterDTO()
        {
            UserName = "testuser",
            Email = "test@gmail.com",
            Name = "Test User",
            Password = "Test.1234"
        };
        var existingUser = new User() { Email = request.Email };
        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.Register(request));
        Assert.Equal("User already exists", ex.Message);
        _mockUserManager.Verify(um => um.FindByEmailAsync(request.Email), Times.Once);
    }

    [Fact]
    public async Task Register_RoleNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new UserRegisterDTO()
        {
            UserName = "testuser2",
            Email = "test2@gmail.com",
            Name = "Test User2",
            Password = "Test.1234"
        };

        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync((User)null);
        _mockRoleManager.Setup(rm => rm.FindByNameAsync("Artist"))
            .ReturnsAsync((Role)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _authService.Register(request));
        Assert.Equal("Role not found", ex.Message);
        _mockRoleManager.Verify(rm => rm.FindByNameAsync("Artist"), Times.Once);
    }

    [Fact]
    public async Task Register_CreateUserFails_ThrowsBadRequestException()
    {
        // Arrange
        var request = new UserRegisterDTO()
        {
            UserName = "testuser3",
            Email = "test3@gmail.com",
            Name = "Test User3",
            Password = "Test.1234"
        };

        var role = new Role() { Id = 3, Name = "Artist" };
        var identityResult = IdentityResult.Failed(new IdentityError { Description = "User registration failed" });

        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync((User)null);
        _mockRoleManager.Setup(rm => rm.FindByNameAsync("Artist"))
            .ReturnsAsync(role);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
            .ReturnsAsync(identityResult);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.Register(request));
        Assert.Contains("User registration failed", ex.Message);
    }

    [Fact]
    public async Task Register_AddToRoleFails_ThrowsBadRequestException()
    {
        // Arrange
        var request = new UserRegisterDTO()
        {
            UserName = "testuser4",
            Email = "test4@gmail.com",
            Name = "Test User4",
            Password = "Test.1234"
        };

        var role = new Role() { Id = 4, Name = "Artist" };

        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync((User)null);
        _mockRoleManager.Setup(rm => rm.FindByNameAsync("Artist"))
            .ReturnsAsync(role);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), role.Name))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" }));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.Register(request));
        Assert.Contains("Role assignment failed", ex.Message);
    }

    [Fact]
    public async Task Login_Success()
    {
        // Arrange
        var request = new UserLoginDTO
        {
            Email = "test@gmail.com",
            Password = "Test.1234"
        };
        var user = new User { Email = request.Email, UserName = "testuser", Id = 1 };
        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        _mockUserManager.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.Login(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
    }

    [Fact]
    public async Task Login_InvalidEmail_ThrowsBadRequestException()
    {
        // Arrange
        var request = new UserLoginDTO
        {
            Email = "notfound@gmail.com",
            Password = "Test.1234"
        };
        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.Login(request));
        Assert.Equal("Invalid email or password", ex.Message);
        _mockUserManager.Verify(um => um.FindByEmailAsync(request.Email), Times.Once);
        _mockUserManager.Verify(um => um.CheckPasswordAsync(It.IsAny<User>(), request.Password), Times.Never);
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsBadRequestException()
    {
        // Arrange
        var request = new UserLoginDTO
        {
            Email = "test@gmail.com",
            Password = "wrongpass"
        };
        var user = new User { Email = request.Email, UserName = "testuser", Id = 1 };
        _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(user, request.Password)).ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.Login(request));
        Assert.Equal("Invalid email or password", ex.Message);
        _mockUserManager.Verify(um => um.FindByEmailAsync(request.Email), Times.Once);
        _mockUserManager.Verify(um => um.CheckPasswordAsync(user, request.Password), Times.Once);
    }

    [Fact]
    public async Task Logout_Success()
    {
        // Arrange
        var userId = "1";
        var user = new User
        {
            Id = 1,
            Email = "test@gmail.com",
            UserName = "testuser",
            RefreshToken = "some_token",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };
        var principal = GetClaimsPrincipal(userId);
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _authService.LogoutAsync(principal);

        // Assert
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiresAt);
        _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Once);
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Logout_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var userId = "1";
        var principal = GetClaimsPrincipal(userId);
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((User)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _authService.LogoutAsync(principal));
        Assert.Equal("User not found", ex.Message);
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Logout_UserNotLoggedIn_ThrowsBadRequestException()
    {
        // Arrange
        var userId = "1";
        var user = new User
        {
            Id = 1,
            Email = "test@gmail.com",
            UserName = "testuser",
            RefreshToken = null,
            RefreshTokenExpiresAt = null
        };
        var principal = GetClaimsPrincipal(userId);
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _authService.LogoutAsync(principal));
        Assert.Equal("User is not logged in", ex.Message);
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Never);
    }

    private static ClaimsPrincipal GetClaimsPrincipal(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        return principal;
    }
}