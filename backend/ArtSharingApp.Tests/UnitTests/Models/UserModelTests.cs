using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class UserModelTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Name_FailsValidation_WhenNullOrEmpty(string name)
    {
        var user = new User { Name = name };
        var context = new ValidationContext(user);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(user, context, true));
    }

    [Fact]
    public void Name_PassesValidation_WhenValid()
    {
        var user = new User { Name = "John Doe" };
        var context = new ValidationContext(user);
        Validator.ValidateObject(user, context, true);
    }

    [Fact]
    public void Name_FailsValidation_WhenTooLong()
    {
        var user = new User { Name = new string('a', 201) };
        var context = new ValidationContext(user);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(user, context, true));
    }

    [Fact]
    public void Name_PassesValidation_WhenMaxLength()
    {
        var user = new User { Name = new string('a', 200) };
        var context = new ValidationContext(user);
        Validator.ValidateObject(user, context, true);
    }

    [Fact]
    public void UpdateBiography_SuccessfullyUpdatesBiography()
    {
        var user = new User { Name = "John Doe" };
        var newBiography = "This is a new biography.";

        user.UpdateBiography(newBiography);
        Assert.Equal(newBiography, user.Biography);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateBiography_ThrowsArgumentException_WhenBiogrpahyIsNullOrEmpty(string biography)
    {
        var user = new User { Name = "John Doe" };
        Assert.Throws<ArgumentException>(() => user.UpdateBiography(biography));
    }

    [Fact]
    public void UpdateProfilePhoto_SuccessfullyUpdatesProfilePhoto()
    {
        var user = new User { Name = "John Doe" };
        var photo = new byte[] { 1, 2, 3, 4 };
        var contentType = "image/jpeg";

        user.UpdateProfilePhoto(photo, contentType);

        Assert.Equal(photo, user.ProfilePhoto);
        Assert.Equal(contentType, user.ContentType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[] { })]
    public void UpdateProfilePhoto_ThrowsArgumentException_WhenPhotoIsNullOrEmpty(byte[] photo)
    {
        var user = new User { Name = "John Doe" };
        var contentType = "image/jpeg";
        Assert.Throws<ArgumentException>(() => user.UpdateProfilePhoto(photo, contentType));
    }

    [Fact]
    public void RemoveProfilePhoto_SuccessfullyRemovesProfilePhoto()
    {
        var user = new User
        {
            Name = "John Doe",
            ProfilePhoto = new byte[] { 1, 2, 3, 4 },
            ContentType = "image/jpeg"
        };

        user.RemoveProfilePhoto();

        Assert.Null(user.ProfilePhoto);
        Assert.Null(user.ContentType);
    }

    [Fact]
    public void SetRefreshToken_SuccessfullySetsRefreshToken()
    {
        var user = new User { Name = "John Doe" };
        var newToken = "new_refresh_token";
        var expiresAt = DateTime.UtcNow.AddDays(7);

        user.SetRefreshToken(newToken, expiresAt);

        Assert.Equal(newToken, user.RefreshToken);
        Assert.Equal(expiresAt, user.RefreshTokenExpiresAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetRefreshToken_ThrowsArgumentException_WhenTokenIsNullOrEmpty(string token)
    {
        var user = new User { Name = "John Doe" };
        var expiresAt = DateTime.UtcNow.AddDays(7);
        Assert.Throws<ArgumentException>(() => user.SetRefreshToken(token, expiresAt));
    }

    [Fact]
    public void SetRefreshToken_ThrowsArgumentException_WhenExpiresAtIsInThePast()
    {
        var user = new User { Name = "John Doe" };
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        Assert.Throws<ArgumentException>(() => user.SetRefreshToken("valid_token", expiresAt));
    }

    [Fact]
    public void ClearRefreshToken_SuccessfullyClearsRefreshToken()
    {
        var user = new User
        {
            Name = "John Doe",
            RefreshToken = "old_refresh_token",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        user.ClearRefreshToken();

        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiresAt);
    }

    [Fact]
    public void IsRefreshTokenValid_ReturnsTrue_WhenTokenIsValid()
    {
        var user = new User
        {
            Name = "John Doe",
            RefreshToken = "refresh_token",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Assert.True(user.IsRefreshTokenValid());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void IsRefreshTokenValid_ReturnsFalse_WhenRefreshTokenIsNullOrEmpty(string refreshToken)
    {
        var user = new User
        {
            Name = "John Doe",
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Assert.False(user.IsRefreshTokenValid());
    }

    [Fact]
    public void IsRefreshTokenValid_ReturnsFalse_WhenRefreshTokenExpiresAtIsNull()
    {
        var user = new User
        {
            Name = "John Doe",
            RefreshToken = "refresh_token",
            RefreshTokenExpiresAt = null
        };
        Assert.False(user.IsRefreshTokenValid());
    }

    [Fact]
    public void IsRefreshTokenValid_ReturnsFalse_WhenRefreshTokenHasExpired()
    {
        var user = new User
        {
            Name = "John Doe",
            RefreshToken = "refresh_token",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(-1)
        };
        Assert.False(user.IsRefreshTokenValid());
    }
}