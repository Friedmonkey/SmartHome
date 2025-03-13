using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface IRoutineService
{
    public record RoutineListResponse(List<Routine> Routines) : Response<RoutineListResponse>;
    public record ActionListResponse(List<DeviceAction> Actions) : Response<ActionListResponse>;

    public record RoutineRequest(Routine routine) : SmartHomeRequest;
    public record DeviceActionRequest(DeviceAction action) : SmartHomeRequest;

    public Task<GuidResponse> CreateRoutine(RoutineRequest request);
    public Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request); // return list of Routines
    public Task<SuccessResponse> UpdateRoutine(RoutineRequest request);
    public Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request);

    public Task<ActionListResponse> GetDeviceActionOfRoutine(SmartHomeGuidRequest request);
    public Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request);
    public Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request);
    public Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request);
}
