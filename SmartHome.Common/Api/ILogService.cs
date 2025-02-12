using SmartHome.Common.Models;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Common.Api;


public interface ILogService
{
    public record LogResponse(List<object> logs) : Response<LogResponse>;
    public record CreateLogRequest(LogType LogType, string Action, DateTime CreateOn, Guid SmartUserId);
    public record RequestByGuid(Guid Id);
    
    public Task<SuccessResponse> CreateLog(CreateLogRequest request);
    
    public Task<SuccessResponse> GetLogsOfSmartHome(RequestByGuid request);
    
    public Task<SuccessResponse> DeleteLog(RequestByGuid request);
    
    public Task<SuccessResponse> UpdateLog(CreateLogRequest request);


}
