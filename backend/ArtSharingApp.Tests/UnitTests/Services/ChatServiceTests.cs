using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using Moq;

namespace ArtSharingApp.Tests.UnitTests.Services;

public class ChatServiceTests
{
    private readonly IChatService _chatService;
    private readonly Mock<IChatRepository> _mockChatRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;

    public ChatServiceTests()
    {
        _mockChatRepository = new Mock<IChatRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();

        _chatService = new ChatService(
            _mockChatRepository.Object,
            _mockUserRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ThrowsBadRequestException_WhenSenderAndReceiverAreSame()
    {
        // Arrange
        int userId = 1;
        string message = "Hello";

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _chatService.SendMessageAsync(userId, userId, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendMessageAsync_ThrowsBadRequestException_WhenMessageIsInvalid(string message)
    {
        // Arrange
        int senderId = 1;
        int receiverId = 2;

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _chatService.SendMessageAsync(senderId, receiverId, message));
    }
}