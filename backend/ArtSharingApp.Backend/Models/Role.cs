using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a user role in the application (e.g., Admin, Artist, User).
/// Inherits from <see cref="IdentityRole{TKey}"/> with integer key.
/// </summary>
public class Role : IdentityRole<int>
{
    /// <summary>
    /// Collection of users assigned to this role.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}