using SmartHome.Common.Models;
using System.Text.Json.Nodes;

namespace SmartHome.Common.Api;


public interface IDeviceTypeService
{
    public record LogResponse(List<object> Logs) : Response<LogResponse>;
    public record CreateLogRequest(string Name, JsonObject DefaultConfig);
    public record RequestByGuid(Guid Id);
    
    public Task<SuccessResponse> CreateLog(CreateLogRequest request);
    
    public Task<SuccessResponse> GetDeviceType(RequestByGuid request);
    
    public Task<SuccessResponse> DeleteLog(RequestByGuid request);
    
    public Task<SuccessResponse> UpdateLog(CreateLogRequest request);


}
