using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a notification sent to a user
/// </summary>
public class Notification
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Text content of the notification
    /// </summary>
    [Required]
    public string Text { get; set; }

    /// <summary>
    /// Date and time when the notification was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Status of the notification. See <see cref="NotificationStatus"/>.
    /// </summary>
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Unique identifier for the user who receives the notification
    /// </summary>
    public int RecipientId { get; set; }

    /// <summary>
    /// Navigation property for the user who receives the notification
    /// </summary>
    public User Recipient { get; set; }

    /// <summary>
    /// Creates a new notification instance with the specified text and recipient ID.
    /// </summary>
    /// <param name="text">
    /// The text content of the notification.
    /// </param>
    /// <param name="recipientId">
    /// The unique identifier of the user who will receive the notification.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="Notification"/> with the specified text and recipient ID,
    /// </returns>
    public static Notification Create(string text, int recipientId)
    {
        return new Notification
        {
            Text = text,
            RecipientId = recipientId,
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.UNREAD
        };
    }

    /// <summary>
    /// Changes the status of the notification
    /// </summary>
    /// <param name="newStatus">
    /// The new status to set for the notification. See <see cref="NotificationStatus"/>.
    /// </param>
    public void ChangeStatus(NotificationStatus newStatus)
    {
        Status = newStatus;
    }
}