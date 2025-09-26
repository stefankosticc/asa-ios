using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class UserServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task AddUserAsync_CreatesUser()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        var dto = new UserRequestDTO
        {
            UserName = "testuser",
            Email = "test@gmail.com",
            Name = "Test User",
            Password = "Test@123",
            RoleId = 1,
            Biography = "This is a test user."
        };

        // Act
        await service.AddUserAsync(dto);

        // Assert
        Assert.True(context.Users.Any(u => u.UserName == "testuser" && u.Email == "test@gmail.com"));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUserWithoutProfilePhoto()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, profilePhoto: new byte[] { 1, 2, 3 }, contentType: "image/png"));
        await context.SaveChangesAsync();

        var dto = new UpdateUserProfileRequestDTO
        {
            Name = "Updated Name",
            RemovePhoto = false
        };

        // Act
        await service.UpdateAsync(1, dto, null);
        var updatedUser = await context.Users.FindAsync(1);
        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated Name", updatedUser.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUserWithProfilePhoto()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();

        var dto = new UpdateUserProfileRequestDTO
        {
            Name = "Updated Name",
            RemovePhoto = false
        };

        var newProfilePhoto = new FormFile(new MemoryStream(new byte[] { 1, 2, 3 }), 0, 3, "Data", "profile.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        // Act
        await service.UpdateAsync(1, dto, newProfilePhoto);
        var updatedUser = await context.Users.FindAsync(1);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated Name", updatedUser.Name);
        Assert.NotNull(updatedUser.ProfilePhoto);
        Assert.Equal("image/png", updatedUser.ContentType);
    }

    [Fact]
    public async Task UpdateAsync_RemovesProfilePhoto()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, profilePhoto: new byte[] { 1, 2, 3 }, contentType: "image/png"));
        await context.SaveChangesAsync();

        var dto = new UpdateUserProfileRequestDTO
        {
            Name = "Updated Name",
            RemovePhoto = true
        };

        // Act
        await service.UpdateAsync(1, dto, null);
        var updatedUser = await context.Users.FindAsync(1);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated Name", updatedUser.Name);
        Assert.Null(updatedUser.ProfilePhoto);
        Assert.Null(updatedUser.ContentType);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ThrowsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetUserByIdAsync(999));
    }

    [Fact]
    public async Task GetUserByUserNameAsync_ReturnsUser()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, userName: "testuser"));
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetUserByUserNameAsync("testuser", loggedInUserId: 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
        Assert.NotNull(result.FollowersCount);
        Assert.NotNull(result.FollowingCount);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_ThrowsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetUserByUserNameAsync("testuser", loggedInUserId: 1));
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsUsers()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetAllUsersAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Id == 1);
        Assert.Contains(result, u => u.Id == 2);
    }

    [Fact]
    public async Task DeleteAsync_DeletesUser()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();

        // Act
        await service.DeleteAsync(1);

        // Assert
        Assert.False(context.Users.Any(u => u.Id == 1));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(999));
    }

    [Fact]
    public async Task UpdateUserBiographyAsync_UpdatesBiography()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, biography: "Old biography"));
        await context.SaveChangesAsync();

        // Act
        await service.UpdateUserBiographyAsync(1, "New biography");
        var updatedUser = await context.Users.FindAsync(1);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("New biography", updatedUser.Biography);
    }

    [Fact]
    public async Task UpdateUserBiographyAsync_ThrowsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateUserBiographyAsync(999, "Biography"));
    }

    [Fact]
    public async Task GetUsersByNameAndUserName_ReturnsUsers()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, name: "Alice", userName: "alice123"));
        context.Users.Add(CreateHelper.CreateUser(2, name: "Bob", userName: "bob456"));
        context.Users.Add(CreateHelper.CreateUser(3, name: "Alex", userName: "alicia789"));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetUsersByNameAndUserName("Ali")).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "Alice" && u.UserName == "alice123");
        Assert.Contains(result, u => u.Name == "Alex" && u.UserName == "alicia789");
    }

    [Fact]
    public async Task GetUsersByNameAndUserName_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, name: "Alice", userName: "alice123"));
        context.Users.Add(CreateHelper.CreateUser(2, name: "Bob", userName: "bob456"));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetUsersByNameAndUserName("Charlie")).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProfilePhotoAsync_ReturnsProfilePhoto()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, profilePhoto: new byte[] { 1, 2, 3 }, contentType: "image/png"));
        await context.SaveChangesAsync();

        // Act
        var (profilePhoto, contentType) = await service.GetProfilePhotoAsync(1);

        // Assert
        Assert.NotNull(profilePhoto);
        Assert.Equal(new byte[] { 1, 2, 3 }, profilePhoto);
        Assert.Equal("image/png", contentType);
    }

    [Fact]
    public async Task GetProfilePhotoAsync_ThrowsNotFound_WhenNoProfilePhoto()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetProfilePhotoAsync(1));
    }

    [Fact]
    public async Task GetProfilePhotoAsync_ReturnsDefaultContentType_WhenContentTypeIsNull()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IUserService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1, profilePhoto: new byte[] { 1, 2, 3 }, contentType: null));
        await context.SaveChangesAsync();

        // Act
        var (profilePhoto, contentType) = await service.GetProfilePhotoAsync(1);

        // Assert
        Assert.NotNull(profilePhoto);
        Assert.Equal(new byte[] { 1, 2, 3 }, profilePhoto);
        Assert.Equal("image/jpeg", contentType);
    }
}