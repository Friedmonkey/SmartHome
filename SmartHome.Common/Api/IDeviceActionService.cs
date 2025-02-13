using SmartHome.Common.Models;
using System.Text.Json.Nodes;

namespace SmartHome.Common.Api;

public interface IDeviceActionService
{
    public record Response(object DeviceAction) : Response<Response>;
    public record DeviceActionsResponse(List<object> DeviceActions) : Response<DeviceActionsResponse>;
    
    public record CreateRequest(string Name, JsonObject Config ,Guid DeviceId, Guid RoutineId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<DeviceActionsResponse> GetDeviceActionsOfSmartHome(RequestByGuid request); // return list of DeviceActions
    
    public Task<SuccessResponse> Delete(RequestByGuid request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
