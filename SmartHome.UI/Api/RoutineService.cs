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
        throw new NotImplementedException();
    }
    public async Task<GuidResponse> CreateRoutine(RoutineRequest request)
    {
        throw new NotImplementedException();
    }
    public async Task<SuccessResponse> DeleteDeviceAction(SmartHomeGuidRequest request)
    {
        throw new NotImplementedException();
    }
    public async Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
        throw new NotImplementedException();
    }
    public async Task<RoutineListResponse> GetAllRoutines(EmptySmartHomeRequest request)
    {
        throw new NotImplementedException();
    }
    public async Task<SuccessResponse> UpdateDeviceAction(DeviceActionRequest request)
    {
        throw new NotImplementedException();
    }
    public async Task<SuccessResponse> UpdateRoutine(RoutineRequest request)
    {
        throw new NotImplementedException();
    }
}
