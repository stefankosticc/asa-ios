namespace ArtSharingApp.Backend.DTO;

// DTO for returning user data to clients
public class UserResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? ProfilePhoto { get; set; }
}

public class UserByUserNameResponseDTO : LoggedInUserDTO
{
    public bool? IsFollowedByLoggedInUser { get; set; }
}

// DTO for creating/updating users (request)
public class UserRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
}

// DTO for user login
public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

// DTO for user registration
public class UserRegisterDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoggedInUserDTO : UserResponseDTO
{
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}

public class UpdateUserBiographyRequestDTO
{
    public string Biography { get; set; }
}

public class UserSearchResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePhoto { get; set; }
}

public class TopArtistResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePhoto { get; set; }
}

public class UpdateUserProfileRequestDTO
{
    public string Name { get; set; }
    public bool RemovePhoto { get; set; } = false;
}