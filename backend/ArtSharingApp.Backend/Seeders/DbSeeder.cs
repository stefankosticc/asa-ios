using ArtSharingApp.Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Seeders;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var roles = new [] {"Admin", "User", "Artist"};
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Role{ Name = role });
            }
        }
    }
}