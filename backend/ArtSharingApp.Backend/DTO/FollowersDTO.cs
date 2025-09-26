namespace ArtSharingApp.Backend.DTO;

public class FollowersDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePhoto { get; set; }
}

public class FollowingDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePhoto { get; set; }
}