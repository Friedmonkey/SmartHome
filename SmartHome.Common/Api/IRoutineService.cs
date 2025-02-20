using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface IRoutineService
{
    public record RoutineListResponse(List<Routine> Routines) : Response<RoutineListResponse>;
    public record ActionListResponse(List<DeviceAction> Actions) : Response<ActionListResponse>;

    public record RoutineRequest(Routine routine) : SmartHomeRequest;
    public record DeviceActionRequest(DeviceAction action) : SmartHomeRequest;

    //public record CreateRoutineRequest(string Name, TimeOnly Start, byte RepeatDays) : SmartHomeRequest;
    //public record UpdateRoutineRequest(Guid Id, string Name, TimeOnly Start, byte RepeatDays) : SmartHomeRequest;
    //public record CreateActionRequest(string Name, string JsonObjectConfig, Guid RoutineId, Guid DeviceId);
    //public record UpdateActionRequest(Guid Id, string Name, string JsonObjectConfig, Guid RoutineId, Guid DeviceId);

    public Task<GuidResponse> CreateRoutine(RoutineRequest request);
    public Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request); // return list of Routines
    public Task<SuccessResponse> UpdateRoutine(RoutineRequest request);
    public Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request);
    
    public Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request);
    public Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request);
    public Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request);
}
