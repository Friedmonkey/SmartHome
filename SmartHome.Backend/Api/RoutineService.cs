using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IRoutineService;
using static SmartHome.Common.SharedConfig.Urls;

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
        return new GuidResponse(result.Entity.Id);
    }
    public async Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request)
    {
        await _ctx.Auth.EnforceIsPartOfSmartHome(request.smartHome);

        var listOfRoutine = await _ctx.DbContext.Routines
        .Where(rs => rs.SmartHomeId == request.smartHome)
        .ToListAsync();

        foreach (var routine in listOfRoutine)
        {
            await _ctx.DbContext.Entry(routine)
                .Collection(r => r.DeviceActions)
                .LoadAsync();
            routine.DeviceActions ??= [];
        }

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
        return SuccessResponse.Success();
    }
    public async  Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.Id);

        await _ctx.DbContext.Routines.Where(r => r.Id == request.Id).ExecuteDeleteAsync();

        return SuccessResponse.Success();
    }
    
    public async Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.action.RoutineId);
        await _ctx.Routine.EnforceDeviceActionInRoutine(request.smartHome, request.action.DeviceId);
        var deviceAction = new DeviceAction()
        {
            JsonObjectConfig = request.action.JsonObjectConfig,
            DeviceId = request.action.DeviceId,
            RoutineId = request.action.RoutineId,
        };
        var result = await _ctx.DbContext.DeviceActions.AddAsync(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();
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
        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.Id);

        var toDelete = await _ctx.DbContext.DeviceActions.Where(da => da.Id == request.Id).FirstOrDefaultAsync();
        if (toDelete != null)
            return SuccessResponse.Failed("Does not exist");

        await _ctx.Routine.EnforceDeviceActionInRoutine(toDelete!.RoutineId, request.Id);
        _ctx.DbContext.DeviceActions.Remove(toDelete!);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
}
