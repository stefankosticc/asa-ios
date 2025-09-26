using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface
{
    /// <summary>
    /// Defines methods for managing chat messages and conversations between users.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Sends a chat message from one user to another and persists it.
        /// </summary>
        /// <param name="senderId">The ID of the user sending the message.</param>
        /// <param name="receiverId">The ID of the user receiving the message.</param>
        /// <param name="message">The message content.</param>
        /// <returns>
        /// The sent message as <see cref="ChatMessageResponseDTO"/>.
        /// </returns>
        /// <exception cref="BadRequestException">Thrown if the message is empty or sent to self.</exception>
        /// <exception cref="NotFoundException">Thrown if sender or receiver is not found.</exception>
        Task<ChatMessageResponseDTO> SendMessageAsync(int senderId, int receiverId, string message);

        /// <summary>
        /// Marks a chat message as read by the receiver.
        /// </summary>
        /// <param name="messageId">The ID of the message to mark as read.</param>
        /// <param name="userId">The ID of the user marking the message as read.</param>
        Task MarkAsReadAsync(int messageId, int userId);

        /// <summary>
        /// Retrieves the chat history between two users.
        /// </summary>
        /// <param name="userId">
        /// The ID of one user.
        /// </param>
        /// <param name="otherUserId">
        /// The ID of the other user.
        /// </param>
        /// <param name="skip">
        /// The number of messages to skip for pagination.
        /// </param>
        /// <param name="take">
        /// The number of messages to take for pagination.
        /// </param>
        /// <returns>
        /// An enumerable collection of <see cref="ChatMessageResponseDTO"/> representing the chat history between the two users.
        /// </returns>
        Task<IEnumerable<ChatMessageResponseDTO>> GetChatHistoryAsync(int userId, int otherUserId, int skip, int take);

        /// <summary>
        /// Returns all users that the logged-in user has had conversations with.
        /// This includes users who have sent messages to the logged-in user or vice versa.
        /// The result is ordered by the last message date, with the most recent conversations first.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user for whom to retrieve conversations.
        /// This should be the ID of the currently logged-in user.
        /// </param>
        /// <param name="skip">
        /// The number of conversations to skip for pagination purposes.
        /// </param>
        /// <param name="take">
        /// The number of conversations to take for pagination purposes.
        /// </param>
        /// <returns>
        /// An enumerable collection of <see cref="UserConversationDTO"/> objects representing the users the provided user has chatted with.
        /// </returns>
        Task<IEnumerable<UserConversationDTO>?> GetConversationsAsync(int userId, int skip, int take);
    }
}