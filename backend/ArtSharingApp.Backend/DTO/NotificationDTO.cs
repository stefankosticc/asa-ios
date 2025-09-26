
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class NotificationRequestDTO
{
    public string Text { get; set; }
    public int RecipientId { get; set; }
}

public class NotificationResponseDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public NotificationStatus Status { get; set; }
}

