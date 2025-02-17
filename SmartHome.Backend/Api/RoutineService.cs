using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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

    public async Task<GuidResponse> CreateRoutine(CreateRoutineRequest request)
    {
        await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);
        var routine = new Routine() 
        { 
            Id = Guid.NewGuid(),
            Name = request.Name,   
            Start = request.Start,
            SmartHomeId = request.smartHome,
            RepeatDays = request.RepeatDays,
        };

        var result = await _ctx.DbContext.Routines.AddAsync(routine);
        await _ctx.DbContext.SaveChangesAsync();
        return new GuidResponse(result.Entity.Id);
    }
    public async Task<RoutineListResponse> GetRoutinesOfSmartHome(SmartHomeRequest request)
    {
        var listOfRoutine = await _ctx.DbContext.Routines
        .Where(rs => rs.SmartHomeId == request.smartHome)
        .ToListAsync();

        if (listOfRoutine is null || listOfRoutine.Count <= 0)
            return RoutineListResponse.Failed("Given request has no valid Routine");

        foreach (var routine in listOfRoutine)
        {
            await _ctx.DbContext.Entry(routine)
                .Collection(r => r.DeviceActions)
                .LoadAsync();
            routine.DeviceActions ??= [];
        }
        return new RoutineListResponse(listOfRoutine);
    }
    public async Task<SuccessResponse> UpdateRoutine(UpdateRoutineRequest request)
    {
        await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);
        var routine = new Routine()
        {
            Id = request.Id,
            Name = request.Name,
            Start = request.Start,
            SmartHomeId = request.smartHome,
            RepeatDays = request.RepeatDays,
        };

        _ctx.DbContext.Routines.Update(routine);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
    public async  Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
        await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);
        var toDelete = await _ctx.DbContext.Routines.Where(rs => rs.Id == request.Id).FirstOrDefaultAsync();
        if(toDelete is null)
        {
            return SuccessResponse.Failed("Given Routine Id is not valid");
        }
        _ctx.DbContext.Routines.Remove(toDelete);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
    
    public async Task<GuidResponse> CreateDeviceAction(CreateActionRequest request)
    {
        var deviceAction = new DeviceAction()
        {
            Name = request.Name,
            JsonObjectConfig = request.JsonObjectConfig,
            DeviceId = request.DeviceId,
            RoutineId = request.RoutineId,
        };
        var result = await _ctx.DbContext.DeviceActions.AddAsync(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();
        return new GuidResponse(result.Entity.Id);
    }
    public async Task<SuccessResponse> UpdateDeviceAction(UpdateActionRequest request)
    {
        var deviceAction = new DeviceAction()
        {
            Id = request.Id,
            Name = request.Name,
            JsonObjectConfig = request.JsonObjectConfig,
            DeviceId = request.DeviceId,
            RoutineId = request.RoutineId,
        };
        _ctx.DbContext.DeviceActions.Update(deviceAction);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDeviceAction(Guid request)
    {
        var toDelete = await _ctx.DbContext.DeviceActions.Where(da =>  da.Id == request).FirstOrDefaultAsync();
        if (toDelete != null)
        {
            return SuccessResponse.Failed("Does not exist");
        }
        _ctx.DbContext.DeviceActions.Remove(toDelete!);
        await _ctx.DbContext.SaveChangesAsync();
        return SuccessResponse.Success();
    }
}
