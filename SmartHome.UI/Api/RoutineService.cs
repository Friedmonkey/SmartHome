using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.UI.Api;
public class RoutineService : IRoutineService
{
    private readonly ApiService _api;

    public RoutineService(ApiService api)
    {
        this._api = api;
    }

    public async Task<GuidResponse> CreateDeviceAction(DeviceActionRequest request)
    {
         return await _api.Post<GuidResponse>(SharedConfig.Urls.Routine.CreateDeviceAction, request);
    }
    public async Task<GuidResponse> CreateRoutine(RoutineRequest request)
    {
         return await _api.Post<GuidResponse>(SharedConfig.Urls.Routine.CreateRoutine, request);
    }
    public async Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request)
    {
         return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Routine.DeleteDeviceAction, request);
    }
    public async Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
         return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Routine.DeleteRoutine, request);
    }
    public async Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request)
    {
         return await _api.Get<RoutineListResponse>(SharedConfig.Urls.Routine.GetAllRoutines, request);
    }
    public async Task<ActionListResponse> GetDeviceActionOfRoutine(SmartHomeGuidRequest request)
    {
        return await _api.Get<ActionListResponse>(SharedConfig.Urls.Routine.GetDeviceActionRoutine, request);
    }
    public async Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request)
    {
         return await _api.Post<SuccessResponse>(SharedConfig.Urls.Routine.UpdateDeviceAction, request);
    }
    public async Task<SuccessResponse> UpdateRoutine(RoutineRequest request)
    {
         return await _api.Post<SuccessResponse>(SharedConfig.Urls.Routine.UpdateRoutine, request);
    }
}
