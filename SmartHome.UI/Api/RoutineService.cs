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

    public Task<GuidResponse> CreateDeviceAction(CreateActionRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<GuidResponse> CreateRoutine(CreateRoutineRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<SuccessResponse> DeleteDeviceAction(Guid request)
    {
        throw new NotImplementedException();
    }
    public Task<SuccessResponse> DeleteRoutine(SmartHomeGuidRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<RoutineListResponse> GetRoutinesOfSmartHome(SmartHomeRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<SuccessResponse> UpdateDeviceAction(UpdateActionRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<SuccessResponse> UpdateRoutine(UpdateRoutineRequest request)
    {
        throw new NotImplementedException();
    }
}
