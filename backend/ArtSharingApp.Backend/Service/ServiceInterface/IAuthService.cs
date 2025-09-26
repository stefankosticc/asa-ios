using System.Security.Claims;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for authentication and authorization services, including user registration, login, token management, and logout.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <param name="request">User registration data.</param>
    /// <exception cref="BadRequestException">Thrown if the user already exists or registration fails.</exception>
    /// <exception cref="NotFoundException">Thrown if the default role is not found.</exception>
    Task Register(UserRegisterDTO request);

    /// <summary>
    /// Logs in a user with the provided credentials and returns JWT tokens.
    /// </summary>
    /// <param name="request">User login data.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> containing access and refresh tokens.</returns>
    /// <exception cref="BadRequestException">Thrown if email or password is invalid.</exception>
    Task<TokenResponseDTO?> Login(UserLoginDTO request);

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="request">Refresh token request data.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> containing new access and refresh tokens.</returns>
    /// <exception cref="BadRequestException">Thrown if the refresh token is invalid.</exception>
    Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);

    /// <summary>
    /// Logs out the user.
    /// </summary>
    /// <param name="userPrincipal">Claims principal representing the logged-in user.</param>
    /// <exception cref="BadRequestException">Thrown if the user is not found or not logged in.</exception>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    Task LogoutAsync(ClaimsPrincipal userPrincipal);
}