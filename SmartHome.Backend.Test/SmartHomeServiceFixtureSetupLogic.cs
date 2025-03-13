using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Backend;
using SmartHome.Backend.Api;
using SmartHome.Backend.Test.Testing;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Configs;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using SmartHome.Database;
using SmartHome.Database.ApiContext;
using SmartHome.Database.Auth;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;
using static SmartHome.Common.Api.IAccountService;

[CollectionDefinition("SmartHomeServiceCollection")]
public class SmartHomeServiceCollection : ICollectionFixture<SmartHomeServiceFixtureSetupLogic> { }

public class SmartHomeServiceFixtureSetupLogic : IDisposable
{
    //public DeviceContext TestDeviceContext { get; }
    //public RoomContext TestRoomContext { get; }
    //public RoutineContext TestRoutineContext { get; }
    public ApiContext TestApiContext { get; }

    public IAccountService TestAccountService { get; }
    public IRoomService TestRoomService { get; }
    public ILogService TestLogService { get; }
    public IRoutineService TestRoutineService { get; }
    public IDeviceService TestDeviceService { get; }
    public ISmartHomeService TestSmartHomeService { get; }
    public IAuthContext TestAuthContext { get; }
    public Guid AccoutId { get; set; }
    public Guid SmartUserId { get; set; } = Guid.NewGuid();
    public Guid SmartHomeId { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; } = Guid.NewGuid();
    public Guid DeviceId { get; set; } = Guid.NewGuid();
    public Guid DeviceAccessId { get; set; } = Guid.NewGuid();
    public Guid DeviceActionId { get; set; } = Guid.NewGuid();
    public Guid RoutineId { get; set; } = Guid.NewGuid();

    //public ITestOutputHelper TestConsole { get; }
    public LoginRequest LoginRequest = new LoginRequest("Admin@gmail.com", "Password1!");

    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _rootScope;
    private readonly IHttpContextAccessor TestHttpContextAccessor;


    public SmartHomeServiceFixtureSetupLogic()//(ITestOutputHelper testConsole)
    {
        // Manually build configuration instead of WebApplication.CreateBuilder([])
        var configuration = new ConfigurationManager()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            //random test secret
            { "JwtSecret", @"6f7ca6118c2fb8f1bcebbe02fe9707d8e2d5b3b67a90d0da56af597cbb400ddda44dd4489ff3ae60543ba9fd634a9a6b8e08686648ec8a848d655e006372217fbd82c7ce8cd3dbf34992c15545a0a911e6a83b486e75677600bcb0eb5fd3a226870d2a2428c65569562474ea908586bbc574b490f69e9c8288dfd85848bf59d64537ad2aa7b7a3b69130524e1dfbdacd557c6855b28c967b86d68d1ee38bd70de76f475f9d2886fd5e7be26d7ab685dfb89eac78595a27052d8b8ef6e01ca75bed0d76293adc21304d73e8ce9c5a59307a59deaf3642abd998205c69169879a65150dd6e3f038ee1d5eab1f99d2f80fba79e2839a8820958edc12676b112331c918b1b890cb184d4001c13eee1825808adebf388f57bbca06206ee4d9d08c4dcfce3f3871bc0d94a05aa8f67d8710d268e4beac53a51a10447f0b501abb6d0f62d8ea6de4832f3c237399113112129be8e182ad8e613ee0840debba7fb15d879d0470c42bb7b29e4788960cb91a8dbed0b8cc96b8dca0d9704e56b1ee71c874cf10ee67e67872445a63902e50fe2a0e6bd46db567c8fe3954d29a326a9bf5d4840fdf444996bfde357e89842ce1947e5e9f258e2d3ae4edc69c66a578217f315" } // Shortened for readability
        })
        .Build();

        var config = new BackendConfig(configuration);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddFastEndpoints();
        services.AddSingleton(configuration);

        services.AddSingleton(config);

        // Use In-Memory database for testing
        services.AddDbContext<SmartHomeContext>(options => options.UseInMemoryDatabase("TestDatabase"));

        // JWT Auth
        services.AddAuthenticationJwtBearer(options => options.SigningKey = config.JwtSecret);
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
        services.AddScoped<IAuthContext, AuthContext>();
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
        _rootScope = _serviceProvider.CreateScope();
        var scopedProvider = _rootScope.ServiceProvider;

        // Use a proper service scope for initialization

        // Resolve services after provider is fully built
        TestAuthContext = scopedProvider.GetRequiredService<IAuthContext>();
        TestApiContext = scopedProvider.GetRequiredService<ApiContext>();
        TestAccountService = scopedProvider.GetRequiredService<IAccountService>();
        TestRoomService = scopedProvider.GetRequiredService<IRoomService>();
        TestLogService = scopedProvider.GetRequiredService<ILogService>();
        TestRoutineService = scopedProvider.GetRequiredService<IRoutineService>();
        TestDeviceService = scopedProvider.GetRequiredService<IDeviceService>();
        TestSmartHomeService = scopedProvider.GetRequiredService<ISmartHomeService>();
        TestHttpContextAccessor = scopedProvider.GetRequiredService<IHttpContextAccessor>();


        var serviceResolver = scopedProvider.GetRequiredService<IServiceResolver>();

        //Get the type of the class that contains the internal static property
        Type myClassType = typeof(FastEndpoints.Config);

        // Get the internal static property using reflection
        PropertyInfo propertyInfo = myClassType.GetProperty("ServiceResolver", BindingFlags.NonPublic | BindingFlags.Static);

        // Access the getter of the property
        if (propertyInfo != null)
        {
            // Get the value of the property (it will throw an exception if _resolver is null)
            try
            {
                var resolverValue = propertyInfo.GetValue(null);
                Console.WriteLine("ServiceResolver value accessed: " + resolverValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing ServiceResolver: {ex.Message}");
            }

            // Set the value of the property (assuming you want to set it to a new instance)
            //var newResolver = new MyNamespace.MyServiceResolver();
            propertyInfo.SetValue(null, serviceResolver);

            // Access again after setting a new value
            var updatedResolver = propertyInfo.GetValue(null);
            Console.WriteLine("Updated ServiceResolver: " + updatedResolver);
        }
        else
        {
            Console.WriteLine("Property not found!");
        }

        var SmartDB = scopedProvider.GetRequiredService<SmartHomeContext>();
        if (SmartDB.Database.EnsureCreated())
        {
            Initialize(SmartDB).GetAwaiter().GetResult();
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
                SmartHomeId = SmartHomeId,
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

    public bool WasSuccess<T>(Response<T>? response) where T : Response<T>
    {   //handles null too
        return response?._RequestSuccess == true;
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}