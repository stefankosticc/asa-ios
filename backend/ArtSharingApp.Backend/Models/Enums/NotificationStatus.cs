namespace ArtSharingApp.Backend.Models.Enums;

/// <summary>
/// Represents the status of a notification.
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// The notification has been read by the user.
    /// </summary>
    READ,

    /// <summary>
    /// The notification has not been read by the user.
    /// </summary>
    UNREAD,

    /// <summary>
    /// The notification has been deleted by the user.
    /// </summary>
    DELETED
}