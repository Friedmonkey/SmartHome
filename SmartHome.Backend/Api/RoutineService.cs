using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using static SmartHome.Common.Api.IRoutineService;

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
        var routine = new Routine() 
        { 
            SmartHomeId = request.smartHome,
            Name = request.routine.Name,   
            Start = request.routine.Start,
            RepeatDays = request.routine.RepeatDays,
        };

        var result = await _ctx.DbContext.Routines.AddAsync(routine);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] created a new routing with name {request.routine.Name}", request, LogType.Action);

        return new GuidResponse(result.Entity.Id);
    }
    public async Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request)
    {
        await _ctx.Auth.EnforceIsPartOfSmartHome(request.smartHome);

        var listOfRoutine = await _ctx.DbContext.Routines
        .Where(rs => rs.SmartHomeId == request.smartHome)
        .ToListAsync();

        //idk what this does
        //foreach (var routine in listOfRoutine)
        //{
        //    await _ctx.DbContext.Entry(routine)
        //        .Collection(r => r.DeviceActions)
        //        .LoadAsync();
        //    routine.DeviceActions ??= [];
        //}

        return new RoutineListResponse(listOfRoutine);
    }
    public async Task<SuccessResponse> UpdateRoutine(RoutineRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.routine.Id);
        await _ctx.Routine.EnforceRoutineNameUnique(request.smartHome, request.routine.Name);

        var routine = await _ctx.DbContext.Routines.FirstOrDefaultAsync(r => r.Id == request.routine.Id && r.SmartHomeId == request.smartHome);
        if (routine is null)
            return SuccessResponse.Failed("Routine does not exist in this smart home!");

        routine.Name = request.routine.Name;
        routine.Start = request.routine.Start;
        routine.RepeatDays = request.routine.RepeatDays;

        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] updated stuff in {request.routine.Name}", request, LogType.Action);

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
    
    public async Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.action.RoutineId);

        throw new ApiError("no fully supported yet");


        if (string.IsNullOrEmpty(request.action.Name)) //why does this couple tabel even have a name??
            return GuidResponse.Failed("Action name cannot be empty"); 
        //if we wanna keep the name we need validation to make sure the name is unique within a routine

        var deviceAction = new DeviceAction()
        {
            Name = request.action.Name,
            JsonObjectConfig = request.action.JsonObjectConfig,
            DeviceId = request.action.DeviceId,
            RoutineId = request.action.RoutineId,
        };

        var result = await _ctx.DbContext.DeviceActions.AddAsync(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] added new action {request.action.Name} to [routine_name]", request, LogType.Action, RoutineId: request.action.RoutineId);

        return new GuidResponse(result.Entity.Id);
    }
    public async Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.action.RoutineId);

        throw new ApiError("no supported yet");

        //TODO: check if this action with this id exists on this routine which we know is part of the smart home that we are admin in


        //TODO: just fix this update by getting it first maby idk
        var deviceAction = new DeviceAction()
        {
            Id = request.action.Id,
            Name = request.action.Name,
            JsonObjectConfig = request.action.JsonObjectConfig,
            DeviceId = request.action.DeviceId,
            RoutineId = request.action.RoutineId,
        };

        _ctx.DbContext.DeviceActions.Update(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();

        await _ctx.CreateLog($"[user] updated routine with name [routine_name] to {request.action.Name}", request, LogType.Action, RoutineId: request.action.RoutineId);

        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Routine.EnforceRoutineInSmartHome(request.smartHome, request.Id);

        //TODO: check if this action exists on this routine
        throw new ApiError("no supported yet");

        var toDelete = await _ctx.DbContext.DeviceActions.Where(da => da.Id == request.Id).FirstOrDefaultAsync();
        if (toDelete != null)
        {
            return SuccessResponse.Failed("Does not exist");
        }

        await _ctx.CreateLog($"[user] deleted action", request, LogType.Action);

        _ctx.DbContext.DeviceActions.Remove(toDelete!);
        await _ctx.DbContext.SaveChangesAsync();

        
        return SuccessResponse.Success();
    }
}
