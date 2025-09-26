using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface
{
    public interface IChatRepository : IGenericRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId, int otherUserId, int skip, int take);
        Task<IEnumerable<User>> GetConversationsAsync(int userId);
    }
}