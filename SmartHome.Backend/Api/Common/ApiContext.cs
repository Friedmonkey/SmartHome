using Microsoft.AspNetCore.Identity;
using SmartHome.Database;
using SmartHome.Database.Auth;
using SmartHome.Database.ApiContext;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Enums;
using SmartHome.Common.Models.Entities;
using SmartHome.Common;

namespace SmartHome.Backend.Api;

public class ApiContext
{
    private readonly UserManager<AuthAccount> _userManager;
    private readonly SignInManager<AuthAccount> _signInManager;
    private readonly BackendConfig _backendConfig;

    private readonly SmartHomeContext _dbContext;

    public readonly AuthContext Auth;
    public readonly DeviceContext Device;
    public readonly RoomContext Room;
    public readonly RoutineContext Routine;

    public ApiContext(
        AuthContext authCtx,
        DeviceContext deviceCtx,
        RoomContext roomCtx,
        //RoutineContext routineCtx,

        SmartHomeContext dbContext//,
        //UserManager<AuthAccount> userManager, SignInManager<AuthAccount> signInManager,
        //BackendConfig backendConfig
        )
    {
        Auth = authCtx;
        Device = deviceCtx;
        Room = roomCtx;
        //Routine = routineCtx;

        _dbContext = dbContext;
        //_userManager = userManager;
        //_signInManager = signInManager;
        //_backendConfig = backendConfig;
    }

    public SmartHomeContext DbContext => _dbContext;
    public UserManager<AuthAccount> UserManager => _userManager;
    public SignInManager<AuthAccount> SignInManager => _signInManager;
    public BackendConfig BackendConfig => _backendConfig;

    public Task SaveDatabase() => _dbContext.SaveChangesAsync();

    public async Task CreateLog(string Action, SmartHomeRequest request, LogType type)
    {
        //Haal de ingelogde smartuser op voor primary key smaruser in de log
        var smartUser = await Auth.GetLoggedInSmartUser(request.smartHome);

        //Zet de log in de database
        var result = await DbContext.Logs.AddAsync(new Log
        {
            Action = Action,
            Type = type,
            CreateOn = DateTime.Now,
            SmartUserId = smartUser.Id,
            SmartHomeId = request.smartHome
        });
    }
}
