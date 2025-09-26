using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class ChatRepository : GenericRepository<ChatMessage>, IChatRepository
{
    public ChatRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId, int otherUserId, int skip, int take)
    {
        return await _dbSet
            .Where(m =>
                (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId && m.ReceiverId == userId))
            .OrderByDescending(m => m.SentAt)
            .Skip(skip)
            .Take(take)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetConversationsAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id != userId &&
                        (u.SentMessages.Any(m => m.ReceiverId == userId) ||
                         u.ReceivedMessages.Any(m => m.SenderId == userId)))
            .Include(u => u.SentMessages)
            .Include(u => u.ReceivedMessages)
            .ToListAsync();
    }
}