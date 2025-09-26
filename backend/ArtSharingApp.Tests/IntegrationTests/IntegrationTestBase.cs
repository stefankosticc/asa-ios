using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArtSharingApp.Backend.DataAccess;
using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.Profile;
using ArtSharingApp.Backend.Seeders;
using ArtSharingApp.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace ArtSharingApp.Tests.IntegrationTests;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected string? ConnectionString { get; private set; }
    protected string? DatabaseName { get; private set; }
    protected IConfigurationRoot Configuration { get; private set; }
    protected ApplicationDbContext? DbContext { get; private set; }
    protected ServiceProvider? ServiceProvider { get; private set; }

    public IntegrationTestBase()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        // Generate a unique database name for each test run
        DatabaseName = $"art-db-test-{Guid.NewGuid():N}";
        var baseConnStr = Configuration.GetConnectionString("ArtSharingAppContext");
        var builder = new NpgsqlConnectionStringBuilder(baseConnStr)
        {
            Database = DatabaseName
        };
        ConnectionString = builder.ToString();
    }

    public async Task InitializeAsync()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(Configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(ConnectionString));

        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IArtworkRepository, ArtworkRepository>();
        services.AddScoped<IGalleryRepository, GalleryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddScoped<IFollowersRepository, FollowersRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IArtworkService, ArtworkService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IGalleryService, GalleryService>();
        services.AddScoped<IFavoritesService, FavoritesService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFollowersService, FollowersService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IChatService, ChatService>();

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddLogging();

        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DbContext.Database.MigrateAsync();
        await DbSeeder.SeedRolesAsync(ServiceProvider);
    }

    public async Task DisposeAsync()
    {
        if (DbContext != null)
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.DisposeAsync();
        }

        if (ServiceProvider is not null && ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}