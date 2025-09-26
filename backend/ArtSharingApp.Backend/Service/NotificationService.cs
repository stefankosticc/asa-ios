using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing notifications.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="notificationRepository">Repository for notification data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public NotificationService(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NotificationResponseDTO>?> GetNotificationsAsync(int loggedInUserId, int skip,
        int take)
    {
        var notifications =
            await _notificationRepository.GetAllReadAndUnreadNotificationsAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);
    }

    /// <inheritdoc />
    public async Task CreateNotificationAsync(NotificationRequestDTO request)
    {
        if (string.IsNullOrEmpty(request.Text) || request.RecipientId <= 0)
            throw new BadRequestException("Invalid notification request.");

        if (await _userRepository.GetByIdAsync(request.RecipientId) == null)
            throw new NotFoundException("Recipient not found.");

        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTime.UtcNow;
        notification.Status = NotificationStatus.UNREAD;

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveAsync();
    }

    /// <inheritdoc />
    public async Task MarkNotificationAsReadAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.READ);
    }

    /// <inheritdoc />
    public async Task MarkNotificationAsUnreadAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.UNREAD);
    }

    /// <inheritdoc />
    public async Task MarkNotificationAsDeletedAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.DELETED);
    }

    /// <summary>
    /// Changes the status of a notification for the logged-in user.
    /// </summary>
    /// <param name="notificationId">The notification ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="status">The new notification status.</param>
    /// <exception cref="NotFoundException">Thrown if the notification is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to change the status.</exception>
    private async Task ChangeNotificationStatus(int notificationId, int loggedInUserId, NotificationStatus status)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            throw new NotFoundException("Notification not found.");

        if (notification.RecipientId != loggedInUserId)
            switch (status)
            {
                case NotificationStatus.READ:
                    throw new UnauthorizedAccessException(
                        "You do not have permission to mark this notification as read.");
                case NotificationStatus.UNREAD:
                    throw new UnauthorizedAccessException(
                        "You do not have permission to mark this notification as unread.");
                case NotificationStatus.DELETED:
                    throw new UnauthorizedAccessException("You do not have permission to delete this notification.");
            }

        notification.ChangeStatus(status);
        _notificationRepository.UpdateNotificationStatus(notification);
        await _notificationRepository.SaveAsync();
    }
}