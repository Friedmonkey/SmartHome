using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Configs;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using static SmartHome.Common.Api.IRoutineService;
using static SmartHome.Common.SharedConfig.Urls;
using Device = SmartHome.Common.Models.Entities.Device;

namespace SmartHome.Backend.Api;

public class RoutineService : IRoutineService
{
    private readonly ApiContext _ctx;

    public RoutineService(ApiContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GuidResponse> CreateRoutine(RoutineRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineNameUnique(request.smartHome, null , request.routine.Name);

        var routine = new Common.Models.Entities.Routine() 
        { 
            SmartHomeId = request.smartHome,
            Name = request.routine.Name,   
            Start = request.routine.Start,
            RepeatDays = request.routine.RepeatDays,
        };

        var result = await _ctx.DbContext.Routines.AddAsync(routine);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] created a new routine with name {request.routine.Name}", request, LogType.Action);

        return new GuidResponse(result.Entity.Id);
    }
    public async Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request)
    {
        await _ctx.Auth.EnforceIsPartOfSmartHome(request.smartHome);

        var listOfRoutine = await _ctx.DbContext.Routines
        .Where(rs => rs.SmartHomeId == request.smartHome)
        .ToListAsync();
        return new RoutineListResponse(listOfRoutine);
    }
    public async Task<SuccessResponse> UpdateRoutine(RoutineRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.routine.Id);
        await _ctx.Routine.EnforceRoutineNameUnique(request.smartHome, request.routine.Id, request.routine.Name);

        var routine = await _ctx.DbContext.Routines.FirstOrDefaultAsync(r => r.Id == request.routine.Id && r.SmartHomeId == request.smartHome);
        if (routine is null)
            return SuccessResponse.Failed("Routine does not exist in this smart home!");

        routine.Name = request.routine.Name;
        routine.Start = request.routine.Start;
        routine.RepeatDays = request.routine.RepeatDays;

        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] updated routine {request.routine.Name}", request, LogType.Action);

        return SuccessResponse.Success();
    }
    public async  Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.Id);

        await _ctx.CreateLog($"[user] deleted routine with name [routine_name]", request, LogType.Action, RoutineId: request.Id);

        await _ctx.DbContext.Routines.Where(r => r.Id == request.Id).ExecuteDeleteAsync();

        return SuccessResponse.Success();
    }
    public async Task<ActionListResponse> GetDeviceActionOfRoutine(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.Id);

        var listOfDeviceAction = await _ctx.DbContext.DeviceActions.Where(r => r.RoutineId == request.Id).ToListAsync();

        foreach (var deviceAction in listOfDeviceAction)
        {
            var device = await _ctx.DbContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceAction.DeviceId);
            Device config = new Device()
            { 
                JsonObjectConfig = deviceAction.JsonObjectConfig,
                Type = device.Type
            };
            config.LoadDeviceConfig();
            deviceAction.Device = device;
        }
        return new ActionListResponse(listOfDeviceAction);
    }

    public async Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.action.RoutineId);
        var deviceAction = new DeviceAction()
        {
            JsonObjectConfig = request.action.JsonObjectConfig,
            DeviceId = request.action.DeviceId,
            RoutineId = request.action.RoutineId,
        };
        var result = await _ctx.DbContext.DeviceActions.AddAsync(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] added new action {request.action} to [routine_name]", request, LogType.Action, RoutineId: request.action.RoutineId);

        return new GuidResponse(result.Entity.Id);
    }
    public async Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.action.RoutineId);
        await _ctx.Routine.EnforceDeviceActionInRoutine(request.action.RoutineId, request.action.Id);

        var deviceAction = new DeviceAction()
        {
            Id = request.action.Id,
            JsonObjectConfig = request.action.JsonObjectConfig,
            DeviceId = request.action.DeviceId,
            RoutineId = request.action.RoutineId,
        };

        _ctx.DbContext.DeviceActions.Update(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] updated routine with name [routine_name] to {request.action}", request, LogType.Action, RoutineId: request.action.RoutineId);

        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);

        var toDelete = await _ctx.DbContext.DeviceActions.Where(da => da.Id == request.Id).FirstOrDefaultAsync();
        if (toDelete == null)
            return SuccessResponse.Failed("Does not exist");

        _ctx.DbContext.DeviceActions.Remove(toDelete!);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
}
