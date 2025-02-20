using Microsoft.AspNetCore.Components;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.Common.Api;

public interface IRoutineService
{
    public record RoutineListResponse(List<Routine> Routines) : Response<RoutineListResponse>;
    public record ActionListResponse(List<Action> Actions) : Response<ActionListResponse>;
    public record CreateRoutineRequest(string Name, TimeOnly Start, byte RepeatDays) : SmartHomeRequest;
    public record UpdateRoutineRequest(Guid Id, string Name, TimeOnly Start, byte RepeatDays) : SmartHomeRequest;
    public record CreateActionRequest(string Name, string JsonObjectConfig, Guid RoutineId, Guid DeviceId) : SmartHomeRequest;
    public record UpdateActionRequest(Guid Id, string Name, string JsonObjectConfig, Guid RoutineId, Guid DeviceId) : SmartHomeRequest;

    public Task<GuidResponse> CreateRoutine(CreateRoutineRequest request);
    public Task<RoutineListResponse> GetRoutinesOfSmartHome(SmartHomeRequest request); // return list of Routines
    public Task<SuccessResponse> UpdateRoutine(UpdateRoutineRequest request);
    public Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request);
    
    public Task<GuidResponse> CreateDeviceAction(CreateActionRequest request);
    public Task<SuccessResponse> UpdateDeviceAction(UpdateActionRequest request);
    public Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request);
}
