using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Backend;
using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Database;
using SmartHome.Database.ApiContext;
using SmartHome.Database.Auth;
using static SmartHome.Common.SharedConfig.Urls;

public class SmartHomeServiceFixtureSetupLogic : IDisposable
{
    public DeviceContext TestDeviceContext { get; }
    public AuthContext TestAuthContext { get; }
    public RoomContext TestRoomContext { get; }
    public RoutineContext TestRoutineContext { get; }
    public ApiContext TestApiContext { get; }

    public IAccountService TestAccountService { get; }
    public IRoomService TestRoomService { get; }
    public ILogService TestLogService { get; }
    public IRoutineService TestRoutineService { get; }
    public IDeviceService TestDeviceService { get; }
    public ISmartHomeService TestSmartHomeService { get; }

    private readonly ServiceProvider _serviceProvider;

    public SmartHomeServiceFixtureSetupLogic()
    {
        var builder = WebApplication.CreateBuilder([]);
        var config = new BackendConfig(builder.Configuration);

        var services = builder.Services;
        services.AddSingleton(config);

        // Use In-Memory database for testing
        services.AddDbContext<SmartHomeContext>(options => options.UseInMemoryDatabase("TestDatabase"));

        // JWT Auth
        services.AddAuthorization();

        // Identity setup
        services.AddIdentity<AuthAccount, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<SmartHomeContext>()
        .AddDefaultTokenProviders();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Register services
        services.AddScoped<DeviceContext>();
        services.AddScoped<AuthContext>();
        services.AddScoped<RoomContext>();
        services.AddScoped<RoutineContext>();
        services.AddScoped<ApiContext>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IRoutineService, RoutineService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<ISmartHomeService, SmartHomeService>();

        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var SmartDB = scope.ServiceProvider.GetRequiredService<SmartHomeContext>();
            if (SmartDB.Database.EnsureCreated())
            {
                Initialize(SmartDB).GetAwaiter().GetResult();
            }
        }

        _serviceProvider = services.BuildServiceProvider();

        // Resolve services and store them in the fixture
        TestDeviceContext = _serviceProvider.GetRequiredService<DeviceContext>();
        TestAuthContext = _serviceProvider.GetRequiredService<AuthContext>();
        TestRoomContext = _serviceProvider.GetRequiredService<RoomContext>();
        TestRoutineContext = _serviceProvider.GetRequiredService<RoutineContext>();
        TestApiContext = _serviceProvider.GetRequiredService<ApiContext>();

        TestAccountService = _serviceProvider.GetRequiredService<IAccountService>();
        TestRoomService = _serviceProvider.GetRequiredService<IRoomService>();
        TestLogService = _serviceProvider.GetRequiredService<ILogService>();
        TestRoutineService = _serviceProvider.GetRequiredService<IRoutineService>();
        TestDeviceService = _serviceProvider.GetRequiredService<IDeviceService>();
        TestSmartHomeService = _serviceProvider.GetRequiredService<ISmartHomeService>();
    }

    public async Task Initialize(SmartHomeContext SmartDB) 
    {
        //todo:make this stuff
    }

    public void Login(Guid guid)
    {
        var accessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        accessor.HttpContext.User = new();
    }

    public void Dispose()
    {
        // Dispose of the service provider to clean up resources
        _serviceProvider.Dispose();
    }
}
