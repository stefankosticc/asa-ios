using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class ChatServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task SendMessageAsync_MessageSent()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IChatService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        await context.SaveChangesAsync();

        // Act
        var result = await service.SendMessageAsync(1, 2, "Hello, this is a test message.");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.SenderId);
        Assert.Equal(2, result.ReceiverId);
        Assert.Equal("Hello, this is a test message.", result.Message);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task SendMessageAsync_ThrowsNotFoundException_WhenSenderOrReceiverNotFound(bool doesSenderExist,
        bool doesReceiverExist)
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IChatService>();
        var context = DbContext!;

        if (doesSenderExist)
            context.Users.Add(CreateHelper.CreateUser(1));

        if (doesReceiverExist)
            context.Users.Add(CreateHelper.CreateUser(2));

        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.SendMessageAsync(1, 2, "Hello, this is a test message."));
    }

    [Fact]
    public async Task MarkAsReadAsync_MessageMarkedAsRead()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IChatService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        context.ChatMessages.Add(ChatMessage.Create(1, 2, "Hello, this is a test message."));
        await context.SaveChangesAsync();

        // Act
        await service.MarkAsReadAsync(1, 2);
        var message = await context.ChatMessages.FindAsync(1);

        // Assert
        Assert.NotNull(message);
        Assert.True(message.IsRead);
    }

    [Fact]
    public async Task GetChatHistoryAsync_ReturnsMessages()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IChatService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        context.Users.Add(CreateHelper.CreateUser(3));
        context.ChatMessages.Add(ChatMessage.Create(1, 2, "Message 1"));
        context.ChatMessages.Add(ChatMessage.Create(2, 1, "Message 2"));
        context.ChatMessages.Add(ChatMessage.Create(1, 2, "Message 3"));
        context.ChatMessages.Add(ChatMessage.Create(1, 3, "Message with another user"));
        await context.SaveChangesAsync();

        // Act
        var messages = await service.GetChatHistoryAsync(1, 2, 0, 10);

        // Assert
        Assert.NotNull(messages);
        var messageList = messages.ToList();
        Assert.Equal(3, messageList.Count);
        Assert.Equal("Message 1", messageList[0].Message);
        Assert.Equal("Message 2", messageList[1].Message);
        Assert.Equal("Message 3", messageList[2].Message);
    }

    [Fact]
    public async Task GetConversationsAsync_ReturnsConversations()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IChatService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        context.Users.Add(CreateHelper.CreateUser(3));
        context.ChatMessages.Add(ChatMessage.Create(1, 2, "Message 1"));
        context.ChatMessages.Add(ChatMessage.Create(2, 1, "Message 2"));
        context.ChatMessages.Add(ChatMessage.Create(1, 3, "Message with another user"));
        await context.SaveChangesAsync();

        // Act
        var conversations = await service.GetConversationsAsync(1, 0, 10);

        // Assert
        Assert.NotNull(conversations);
        var conversationList = conversations!.ToList();
        Assert.Equal(2, conversationList.Count);
        Assert.Contains(conversationList, c => c.UserId == 2);
        Assert.Contains(conversationList, c => c.UserId == 3);
    }
}