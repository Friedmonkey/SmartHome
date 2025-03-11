using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Backend;
using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Configs;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using SmartHome.Database;
using SmartHome.Database.ApiContext;
using SmartHome.Database.Auth;
using SmartHome.UI.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IAccountService;

[CollectionDefinition("SmartHomeServiceCollection")]
public class SmartHomeServiceCollection : ICollectionFixture<SmartHomeServiceFixtureSetupLogic> { }

public class SmartHomeServiceFixtureSetupLogic : IDisposable
{
    //public DeviceContext TestDeviceContext { get; }
    //public AuthContext TestAuthContext { get; }
    //public RoomContext TestRoomContext { get; }
    //public RoutineContext TestRoutineContext { get; }
    public ApiContext TestApiContext { get; }

    public IAccountService TestAccountService { get; }
    public IRoomService TestRoomService { get; }
    public ILogService TestLogService { get; }
    public IRoutineService TestRoutineService { get; }
    public IDeviceService TestDeviceService { get; }
    public ISmartHomeService TestSmartHomeService { get; }
    public IJwtStoreService _jwtStoreService { get; }
    public Guid AccoutId { get; set; }
    public Guid SmartUserId { get; set; } = Guid.NewGuid();
    public Guid SmartHomeId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; } = Guid.NewGuid();
    public Guid DeviceId { get; set; } = Guid.NewGuid();
    public Guid DeviceAccessId { get; set; } = Guid.NewGuid();
    public Guid DeviceActionId { get; set; } = Guid.NewGuid();
    public Guid RoutineId { get; set; } = Guid.NewGuid();

    //public ITestOutputHelper TestConsole { get; }

    private readonly ServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor TestHttpContextAccessor;


    public SmartHomeServiceFixtureSetupLogic()//(ITestOutputHelper testConsole)
    {
        //TestConsole = testConsole;
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

        // Now build the ServiceProvider
        _serviceProvider = services.BuildServiceProvider();

        // Use a proper service scope for initialization

        // Resolve services after provider is fully built
        TestApiContext = _serviceProvider.GetRequiredService<ApiContext>();
        TestAccountService = _serviceProvider.GetRequiredService<IAccountService>();
        TestRoomService = _serviceProvider.GetRequiredService<IRoomService>();
        TestLogService = _serviceProvider.GetRequiredService<ILogService>();
        TestRoutineService = _serviceProvider.GetRequiredService<IRoutineService>();
        TestDeviceService = _serviceProvider.GetRequiredService<IDeviceService>();
        TestSmartHomeService = _serviceProvider.GetRequiredService<ISmartHomeService>();
        TestHttpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        using (var scope = _serviceProvider.CreateScope())
        {
            var SmartDB = scope.ServiceProvider.GetRequiredService<SmartHomeContext>();
            if (SmartDB.Database.EnsureCreated())
            {
                Initialize(SmartDB).GetAwaiter().GetResult();
            }
        }
    }


    public async Task Initialize(SmartHomeContext SmartDB)
    {
        var _ctx = TestApiContext;
        var request = new RegisterRequest("Admin@gmail.com", "Admin", "Password1!", "Password1!");
        var result = await TestAccountService.Register(request);
        var account = new AuthAccount()
        {
            Email = "Admin@gmail.com",
            UserName = "Admin",
            EmailConfirmed = true,
        };
        await SmartDB.SaveChangesAsync();
        var user = await _ctx.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new NullReferenceException(nameof(user));
        AccoutId = user!.Id;

        await SmartDB.SmartHomes.AddAsync(
            new SmartHomeModel()
            {
                Id = SmartHomeId,
                Name = "admin",
                SSID = "admin",
                SSPassword = "admin",
            }
        );
        await SmartDB.SmartUsers.AddAsync(
            new SmartUserModel()
            {
                Id = SmartUserId,
                AccountId = AccoutId,
                Role = UserRole.Admin,
                SmartHomeId = SmartHomeId,
            }
        );
        await SmartDB.Rooms.AddAsync(
            new Room()
            {
                Id = RoomId,
                Name = "admin",
                SmartHomeId = SmartHomeId,
            }
        );
        var device = new Device()
        {
            Id = DeviceId,
            Name = "admin",
            RoomId = RoomId,
            Type = DeviceType.Lamp,
            Config = new LampConfig()
            {
                Brightness = 50,
                Color = "#FF0000",
                Enabled = true,
            }
        };
        device.SaveDeviceConfig();
        await SmartDB.Devices.AddAsync(device);

        await SmartDB.DeviceAccesses.AddAsync(
        new DeviceAccess()
        {
                Id = DeviceAccessId,
                SmartUserId = SmartUserId,
                DeviceId = DeviceId,
        }
        );

        await SmartDB.Routines.AddAsync(
            new Routine()
            {
                Id = RoutineId,
                Name = "admin",
                RepeatDays = (byte)(RoutineRepeat.Monday | RoutineRepeat.Wednsday),
                SmartHomeId = RoutineId,
                Start = TimeOnly.MinValue
            }
        );

        await SmartDB.DeviceActions.AddAsync(
            new DeviceAction()
            {
                Id = DeviceActionId,
                Name = "admin",
                DeviceId = device.Id,
                RoutineId = RoutineId,
                JsonObjectConfig = device.JsonObjectConfig,
            }
        );
        await SmartDB.SaveChangesAsync();
    }


    public void ApiLogin(string jwtStr)
    {
        var jwt = new JwtSecurityToken(jwtStr);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        TestHttpContextAccessor.HttpContext ??= new DefaultHttpContext();
        TestHttpContextAccessor.HttpContext.User = user;
    }


    public void Dispose()
    {
        // Dispose of the service provider to clean up resources
        _serviceProvider.Dispose();
    }
}
