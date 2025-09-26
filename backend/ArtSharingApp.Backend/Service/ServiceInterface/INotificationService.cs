using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Retrieves all read and unread notifications for the logged-in user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="skip">The number of notifications to skip for pagination. Default is <c>0</c>.</param>
    /// <param name="take">The number of notifications to take for pagination. Default is <c>10</c>.</param>
    /// <returns>A collection of <see cref="NotificationResponseDTO"/> representing the notifications for the user.</returns>
    Task<IEnumerable<NotificationResponseDTO>?> GetNotificationsAsync(int loggedInUserId, int skip = 0, int take = 10);

    /// <summary>
    /// Creates a new notification for a user.
    /// </summary>
    /// <param name="request">Notification creation data.</param>
    /// <exception cref="BadRequestException">Thrown if the request is invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the recipient user is not found.</exception>
    Task CreateNotificationAsync(NotificationRequestDTO request);

    /// <summary>
    /// Marks a notification as read for the logged-in user.
    /// </summary>
    /// <param name="notificationId">The notification ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    Task MarkNotificationAsReadAsync(int notificationId, int loggedInUserId);

    /// <summary>
    /// Marks a notification as unread for the logged-in user.
    /// </summary>
    /// <param name="notificationId">The notification ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    Task MarkNotificationAsUnreadAsync(int notificationId, int loggedInUserId);

    /// <summary>
    /// Marks a notification as deleted for the logged-in user.
    /// </summary>
    /// <param name="notificationId">The notification ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    Task MarkNotificationAsDeletedAsync(int notificationId, int loggedInUserId);
}