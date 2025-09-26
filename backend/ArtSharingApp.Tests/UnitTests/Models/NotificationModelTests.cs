using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class NotificationModelTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Notification_FailsValidation_WhenTextIsNullOrEmpty(string text)
    {
        var notification = new Notification
        {
            Text = text,
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.UNREAD,
            RecipientId = 1
        };

        var context = new ValidationContext(notification);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(notification, context, true));
    }

    [Fact]
    public void Notification_ValidatesSuccessfully_WhenTextIsValid()
    {
        var notification = new Notification
        {
            Text = "This is a valid notification text.",
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.UNREAD,
            RecipientId = 1
        };
        var context = new ValidationContext(notification);
        Validator.ValidateObject(notification, context, true);
    }

    [Fact]
    public void ChangeStatus_UpdatesStatusCorrectly()
    {
        var notification = new Notification
        {
            Text = "Initial notification",
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.UNREAD,
            RecipientId = 1
        };

        notification.ChangeStatus(NotificationStatus.READ);
        Assert.Equal(NotificationStatus.READ, notification.Status);
    }

    [Fact]
    public void Create_CreatesNotificationWithCorrectProperties()
    {
        var notification = Notification.Create("New notification", 1);
        Assert.NotNull(notification);
        Assert.Equal("New notification", notification.Text);
        Assert.Equal(1, notification.RecipientId);
        Assert.Equal(NotificationStatus.UNREAD, notification.Status);
        Assert.Equal(DateTime.UtcNow, notification.CreatedAt, TimeSpan.FromSeconds(1));
    }
}