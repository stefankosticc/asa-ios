using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class FollowersModelTests
{
    [Fact]
    public void Constructor_SetsUserIdAndFollowerId()
    {
        var followers = new Followers(1, 2);
        Assert.Equal(1, followers.UserId);
        Assert.Equal(2, followers.FollowerId);
    }
}