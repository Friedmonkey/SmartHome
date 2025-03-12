using Microsoft.AspNetCore.Identity;
using SmartHome.Database;
using SmartHome.Database.Auth;
using SmartHome.Database.ApiContext;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Enums;
using SmartHome.Common.Models.Entities;
using SmartHome.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
        RoutineContext routineCtx,

        SmartHomeContext dbContext,
        UserManager<AuthAccount> userManager, SignInManager<AuthAccount> signInManager,
        BackendConfig backendConfig)
    {
        Auth = authCtx;
        Device = deviceCtx;
        Room = roomCtx;
        Routine = routineCtx;

        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _backendConfig = backendConfig;
    }

    public SmartHomeContext DbContext => _dbContext;
    public UserManager<AuthAccount> UserManager => _userManager;
    public SignInManager<AuthAccount> SignInManager => _signInManager;
    public BackendConfig BackendConfig => _backendConfig;

    public Task SaveDatabase() => _dbContext.SaveChangesAsync();

    public async Task CreateLog(string Action, SmartHomeRequest request, LogType type, Guid DeviceId = new Guid(), Guid RoomId = new Guid(), Guid RoutineId = new Guid())
    {
        //Haal de logService op
        LogService logService = new LogService(this);
        EmptySmartHomeRequest smartHomeRequest = new EmptySmartHomeRequest
        {
            smartHome = request.smartHome
        };

        Device device = new Device();

        if (DeviceId != Guid.Empty)
        {
            var result = await _dbContext.Devices.Where(d => d.Id == DeviceId).ToListAsync();
            device = result.FirstOrDefault();
            Action = Action.Replace("[device_name]", device.Name);
            Action = Action.Replace("[device_room]", device.Room.Name);

            string TypeTest = device.Type.ToString();

            switch (device.Type)
            {
                case DeviceType.Lamp:
                    Action = Action.Replace("[device_type]", "Lamp");
                  break;
                case DeviceType.Wasmachine:
                    Action = Action.Replace("[device_type]", "Wasmachine");
                    break;
                case DeviceType.Televisie:
                    Action = Action.Replace("[device_type]", "Televisie");
                    break;
                case DeviceType.Router:
                    Action = Action.Replace("[device_type]", "Router");
                    break;
            }
        }

        Room room = new Room();

        if (RoomId != Guid.Empty)
        {
            var result = await _dbContext.Rooms.Where(r => r.Id == RoomId).ToListAsync();
            room = result.FirstOrDefault();
            Action = Action.Replace("[room_name]", room.Name);
        }

        Routine routine = new Routine();

        if (RoutineId != Guid.Empty)
        {
            var result = await _dbContext.Routines.Where(r => r.Id == RoomId).ToListAsync();
            routine = result.FirstOrDefault();
            Action = Action.Replace("[routine_name]", routine.Name);
        }

        //Haal de smartuser uit de database en verander de {user} parameter naar de user naam
        var user = await Auth.GetLoggedInAccount();
        Action = Action.Replace("[user]", user.UserName);

        Log newLog = new Log
        {
            Action = Action,
            Type = type,
            CreateOn = DateTime.Now,
            SmartUserId = user.Id,
            SmartHomeId = request.smartHome
        };

        //Zet de log in de database
        await logService.CreateLog(new(newLog));

    }
}
