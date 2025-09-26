using ArtSharingApp.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Favorites> Favorites { get; set; }
    public DbSet<Followers> Followers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        modelBuilder.Entity<Gallery>()
            .HasOne(g => g.City)
            .WithMany(c => c.Galleries)
            .HasForeignKey(g => g.CityId)
            .IsRequired();

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.City)
            .WithMany(c => c.Artworks)
            .HasForeignKey(a => a.CityId)
            .IsRequired(false);

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.Gallery)
            .WithMany(g => g.Artworks)
            .HasForeignKey(a => a.GalleryId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.CreatedByArtist)
            .WithMany(u => u.CreatedArtworks)
            .HasForeignKey(a => a.CreatedByArtistId)
            .IsRequired();

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.PostedByUser)
            .WithMany(u => u.PostedArtworks)
            .HasForeignKey(a => a.PostedByUserId)
            .IsRequired();

        modelBuilder.Entity<Favorites>()
            .HasKey(f => new { f.UserId, f.ArtworkId });

        modelBuilder.Entity<Favorites>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .IsRequired();

        modelBuilder.Entity<Favorites>()
            .HasOne(f => f.Artwork)
            .WithMany(a => a.Favorites)
            .HasForeignKey(f => f.ArtworkId)
            .IsRequired();

        modelBuilder.Entity<Followers>()
            .HasKey(f => new { f.UserId, f.FollowerId });

        modelBuilder.Entity<Followers>()
            .HasOne(f => f.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.UserId)
            .IsRequired();

        modelBuilder.Entity<Followers>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .IsRequired();

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Recipient)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.RecipientId)
            .IsRequired();

        modelBuilder.Entity<Auction>()
            .HasOne(ac => ac.Artwork)
            .WithMany(a => a.Auctions)
            .HasForeignKey(ac => ac.ArtworkId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Offer>()
            .HasOne(o => o.Auction)
            .WithMany(ac => ac.Offers)
            .HasForeignKey(o => o.AuctionId)
            .IsRequired();

        modelBuilder.Entity<Offer>()
            .HasOne(o => o.User)
            .WithMany(u => u.Offers)
            .HasForeignKey(o => o.UserId)
            .IsRequired();

        modelBuilder.Entity<Artwork>()
            .Property(a => a.Currency)
            .HasConversion<string>();

        modelBuilder.Entity<Auction>()
            .Property(ac => ac.Currency)
            .HasConversion<string>();

        // Artwork indexes
        modelBuilder.Entity<Artwork>()
            .HasIndex(a => a.IsPrivate)
            .HasDatabaseName("IX_Artwork_IsPrivate");

        modelBuilder.Entity<Artwork>()
            .HasIndex(a => new { a.IsPrivate, a.Date, a.CreatedByArtistId })
            .HasDatabaseName("IX_Artwork_IsPrivate_Date_CreatedByArtistId");

        // Favorites index
        modelBuilder.Entity<Favorites>()
            .HasIndex(f => f.ArtworkId)
            .HasDatabaseName("IX_Favorites_ArtworkId");

        // Auctions index
        modelBuilder.Entity<Auction>()
            .HasIndex(a => new { a.StartTime, a.EndTime })
            .HasDatabaseName("IX_Auction_StartTime_EndTime");

        // Offers index
        modelBuilder.Entity<Offer>()
            .HasIndex(o => new { o.AuctionId, o.Amount })
            .HasDatabaseName("IX_Offer_AuctionId_Amount");

        // Followers index
        modelBuilder.Entity<Followers>()
            .HasIndex(f => f.FollowerId)
            .HasDatabaseName("IX_Followers_FollowerId");

        // ChatMessage configuration
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Message)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(m => new { m.SenderId, m.ReceiverId, m.SentAt })
                .HasDatabaseName("IX_ChatMessage_Sender_Receiver_SentAt");
        });
    }
}