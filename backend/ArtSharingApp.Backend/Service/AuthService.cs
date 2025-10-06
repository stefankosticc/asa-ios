using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides authentication and authorization services, including user registration, login, token management, and logout.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userManager">User manager for user operations.</param>
    /// <param name="roleManager">Role manager for role operations.</param>
    /// <param name="configuration">Application configuration for JWT settings.</param>
    public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task Register(UserRegisterDTO request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new BadRequestException("User already exists");

        var role = await _roleManager.FindByNameAsync("Artist");
        if (role == null)
            throw new NotFoundException("Role not found");

        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            RoleId = role.Id
        };

        // Create user
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"User registration failed: {errorMessages}");
        }

        // Assign role to user
        var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
        if (!roleResult.Succeeded)
            throw new BadRequestException("Role assignment failed");
    }

    /// <inheritdoc />
    public async Task<TokenResponseDTO?> Login(UserLoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new BadRequestException("Invalid email or password");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
            throw new BadRequestException("Invalid email or password");

        var response = new TokenResponseDTO()
        {
            AccessToken = await GenerateJwtToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
        return response;
    }

    /// <inheritdoc />
    public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user == null || !user.IsRefreshTokenValid())
            throw new BadRequestException("Invalid refresh token");

        var newRefreshToken = await GenerateAndSaveRefreshTokenAsync(user);
        var newAccessToken = await GenerateJwtToken(user);

        return new TokenResponseDTO()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    /// <inheritdoc />
    public async Task LogoutAsync(ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new BadRequestException("User not found");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (user.RefreshToken == null)
            throw new BadRequestException("User is not logged in");

        user.ClearRefreshToken();
        await _userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Generates a JWT access token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>The generated JWT token as a string.</returns>
    private async Task<string> GenerateJwtToken(User user)
    {
        var role = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role.FirstOrDefault() ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    /// <summary>
    /// Generates a secure random refresh token.
    /// </summary>
    /// <returns>The generated refresh token as a string.</returns>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Generates and saves a new refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate and save the refresh token.</param>
    /// <returns>The generated refresh token as a string.</returns>
    /// <exception cref="BadRequestException">Thrown if refresh token assignment fails.</exception>
    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        try
        {
            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }

        await _userManager.UpdateAsync(user);
        return refreshToken;
    }
}