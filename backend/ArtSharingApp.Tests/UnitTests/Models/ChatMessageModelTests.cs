using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class ChatMessageModelTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Message_FailsValidationWhenNullOrEmpty(string message)
    {
        var chatMessage = new ChatMessage
        {
            SenderId = 1,
            ReceiverId = 2,
            Message = message
        };

        var context = new ValidationContext(chatMessage);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(chatMessage, context, true));
    }

    [Fact]
    public void SentAt_DefaultsToCurrentUtcTime()
    {
        var chatMessage = new ChatMessage
        {
            SenderId = 1,
            ReceiverId = 2,
            Message = "Hello"
        };
        Assert.Equal(DateTime.UtcNow, chatMessage.SentAt, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CreateChatMessage_Successfully()
    {
        var chatMessage = ChatMessage.Create(1, 2, "Hello");
        Assert.NotNull(chatMessage);
        Assert.Equal(1, chatMessage.SenderId);
        Assert.Equal(2, chatMessage.ReceiverId);
        Assert.Equal("Hello", chatMessage.Message);
        Assert.Equal(DateTime.UtcNow, chatMessage.SentAt, TimeSpan.FromSeconds(1));
        Assert.False(chatMessage.IsRead);
    }

    [Fact]
    public void MarkAsRead_SuccessfullyMarksMessageAsRead()
    {
        var chatMessage = new ChatMessage
        {
            SenderId = 1,
            ReceiverId = 2,
            Message = "Hello"
        };

        chatMessage.MarkAsRead();

        Assert.True(chatMessage.IsRead);
    }
}