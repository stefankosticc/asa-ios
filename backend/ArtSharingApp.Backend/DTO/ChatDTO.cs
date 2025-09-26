namespace ArtSharingApp.Backend.DTO;

public class ChatMessageResponseDTO
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}

public class SendChatMessageRequestDTO
{
    public int ReceiverId { get; set; }
    public string Message { get; set; }
}

public class UserConversationDTO
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePhoto { get; set; }
    public int UnreadMessageCount { get; set; }
    public string LastMessage { get; set; }
    public DateTime LastMessageDateTime { get; set; }
}