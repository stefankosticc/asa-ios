using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.Extensions.DependencyInjection;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class NotificationServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task GetNotifications_ReturnsEmpty_WhenNoNotificationsExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();

        // Act
        var result = await service.GetNotificationsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetNotifications_ReturnsNotifications_WhenNotificationsExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        context.Notifications.Add(Notification.Create("New follower", 1));
        context.Notifications.Add(Notification.Create("New message", 1));
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetNotificationsAsync(1)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, n => n.Text == "New follower");
        Assert.Contains(result, n => n.Text == "New message");
    }

    [Fact]
    public async Task CreateNotification_CreatesNotification()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();
        var dto = new NotificationRequestDTO
        {
            Text = "New follower",
            RecipientId = 1
        };

        // Act
        await service.CreateNotificationAsync(dto);

        // Assert
        Assert.True(context.Notifications.Any(n => n.Text == "New follower" && n.RecipientId == 1));
    }

    [Fact]
    public async Task CreateNotification_ThrowsNotFoundException_WhenRecipientDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();
        var dto = new NotificationRequestDTO
        {
            Text = "New follower",
            RecipientId = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateNotificationAsync(dto));
    }

    [Theory]
    [InlineData(NotificationStatus.UNREAD)]
    [InlineData(NotificationStatus.READ)]
    [InlineData(NotificationStatus.DELETED)]
    public async Task MarkNotificationAsX_Success(NotificationStatus status)
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var notification = Notification.Create("New follower", 1);
        context.Notifications.Add(notification);
        await context.SaveChangesAsync();

        // Act
        switch (status)
        {
            case NotificationStatus.UNREAD:
                await service.MarkNotificationAsUnreadAsync(notification.Id, 1);
                break;
            case NotificationStatus.READ:
                await service.MarkNotificationAsReadAsync(notification.Id, 1);
                break;
            case NotificationStatus.DELETED:
                await service.MarkNotificationAsDeletedAsync(notification.Id, 1);
                break;
        }

        // Assert
        var updatedNotification = await context.Notifications.FindAsync(notification.Id);
        Assert.Equal(status, updatedNotification.Status);
    }

    [Fact]
    public async Task MarkNotificationAsX_ThrowsNotFoundException_WhenNotificationDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.MarkNotificationAsReadAsync(999, 1));
        await Assert.ThrowsAsync<NotFoundException>(() => service.MarkNotificationAsUnreadAsync(999, 1));
        await Assert.ThrowsAsync<NotFoundException>(() => service.MarkNotificationAsDeletedAsync(999, 1));
    }

    [Fact]
    public async Task MarkNotificationAsX_ThrowsUnauthorizedAccessExceptionn_WhenUserIsNotRecipient()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<INotificationService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        var notification = Notification.Create("New follower", 1);
        context.Notifications.Add(notification);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.MarkNotificationAsReadAsync(notification.Id, 2));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.MarkNotificationAsUnreadAsync(notification.Id, 2));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.MarkNotificationAsDeletedAsync(notification.Id, 2));
    }
}