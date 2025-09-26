using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class FollowersServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task FollowUserAsync_FollowsUserSuccessfully()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        context.Users.Add(user1);
        context.Users.Add(user2);
        await context.SaveChangesAsync();

        // Act
        var result = await service.FollowUserAsync(user1.Id, user2.Id);

        // Assert
        Assert.True(result);
        Assert.True(context.Followers.Any(f => f.UserId == user1.Id && f.FollowerId == user2.Id));
        Assert.True(context.Notifications.Any(n => n.RecipientId == user2.Id));
    }

    [Fact]
    public async Task FollowUserAsync_ThrowsBadRequest_WhenAlreadyFollowing()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.FollowUserAsync(user1.Id, user2.Id));
    }

    [Fact]
    public async Task UnfollowUserAsync_UnfollowsUserSuccessfully()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        await context.SaveChangesAsync();

        // Act
        var result = await service.UnfollowUserAsync(user1.Id, user2.Id);

        // Assert
        Assert.True(result);
        Assert.False(context.Followers.Any(f => f.UserId == user1.Id && f.FollowerId == user2.Id));
    }

    [Fact]
    public async Task UnfollowUserAsync_ThrowsBadRequest_WhenNotFollowing()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        context.Users.Add(user1);
        context.Users.Add(user2);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UnfollowUserAsync(user1.Id, user2.Id));
    }

    [Fact]
    public async Task GetFollowersAsync_ReturnsFollowers()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1, name: "user1");
        var user2 = CreateHelper.CreateUser(2, name: "user2");
        var user3 = CreateHelper.CreateUser(3, name: "user3");
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        context.Followers.Add(new Followers(user3.Id, user2.Id));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetFollowersAsync(user2.Id, 0, 10))!.ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Name == user1.Name);
        Assert.Contains(result, f => f.Name == user3.Name);
    }

    [Fact]
    public async Task GetFollowersAsync_ThrowsNotFound_WhenNoFollowersFound()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        context.Users.Add(user1);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetFollowersAsync(1, 0, 10));
    }

    [Fact]
    public async Task GetFollowingAsync_ReturnsFollowing()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1, name: "user1");
        var user2 = CreateHelper.CreateUser(2, name: "user2");
        var user3 = CreateHelper.CreateUser(3, name: "user3");
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        context.Followers.Add(new Followers(user1.Id, user3.Id));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetFollowingAsync(user1.Id, 0, 10))!.ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Name == user2.Name);
        Assert.Contains(result, f => f.Name == user3.Name);
    }

    [Fact]
    public async Task GetFollowingAsync_ThrowsNotFound_WhenNoFollowingFound()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        context.Users.Add(user1);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetFollowingAsync(1, 0, 10));
    }

    [Fact]
    public async Task GetFollowersCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        var user3 = CreateHelper.CreateUser(3);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        context.Followers.Add(new Followers(user3.Id, user2.Id));
        await context.SaveChangesAsync();

        // Act
        var count = await service.GetFollowersCountAsync(user2.Id);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetFollowersCountAsync_ReturnsZero_WhenNoFollowers()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        context.Users.Add(user1);
        await context.SaveChangesAsync();

        // Act
        var count = await service.GetFollowersCountAsync(user1.Id);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task GetFollowingCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        var user3 = CreateHelper.CreateUser(3);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.Followers.Add(new Followers(user1.Id, user2.Id));
        context.Followers.Add(new Followers(user1.Id, user3.Id));
        await context.SaveChangesAsync();

        // Act
        var count = await service.GetFollowingCountAsync(user1.Id);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetFollowingCountAsync_ReturnsZero_WhenNoFollowing()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        context.Users.Add(user1);
        await context.SaveChangesAsync();

        // Act
        var count = await service.GetFollowingCountAsync(user1.Id);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task GetFollowedUsersArtworksAsync_ReturnsArtworks()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IFollowersService>();
        var context = DbContext!;

        var user1 = CreateHelper.CreateUser(1);
        var user2 = CreateHelper.CreateUser(2);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Followers.Add(new Followers(user1.Id, user2.Id));

        var artwork1 = CreateHelper.CreateArtwork("Artwork1", user2.Id, user2.Id);
        var artwork2 = CreateHelper.CreateArtwork("Artwork2", user2.Id, user2.Id);
        context.Artworks.Add(artwork1);
        context.Artworks.Add(artwork2);

        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetFollowedUsersArtworksAsync(user1.Id, 0, 10))!.ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, a => a.Title == artwork1.Title);
        Assert.Contains(result, a => a.Title == artwork2.Title);
    }
}