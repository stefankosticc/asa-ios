namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a relationship where a user follows another user
/// </summary>
public class Followers
{
    /// <summary>
    /// Unique identifier for the user who is following <see cref="FollowerId"/>
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user who is following <see cref="FollowerId"/>
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Unique identifier for the user who is being followed
    /// <remarks>
    /// This is the user that <see cref="UserId"/> follows.
    /// </remarks>
    /// </summary>
    public int FollowerId { get; set; }

    /// <summary>
    /// Navigation property to the user who is being followed
    /// </summary>
    public User Follower { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Followers"/> class with specified user and follower IDs.
    /// <remarks>
    /// This constructor is used to create a relationship where one user follows another.
    /// </remarks>
    /// </summary>
    /// <param name="userId">
    /// Unique identifier for the user who is following another user
    /// </param>
    /// <param name="followerId">
    /// Unique identifier for the user who is being followed
    /// </param>
    public Followers(int userId, int followerId)
    {
        UserId = userId;
        FollowerId = followerId;
    }
}