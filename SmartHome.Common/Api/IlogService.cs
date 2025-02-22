using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record LogListResponse(List<Log> Logs) : Response<LogListResponse>;

public interface ILogService
{
    Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request);

    public record CreateLogRequest(string Action, string Type) : SmartHomeRequest;
    Task<SuccessResponse> CreateLog(CreateLogRequest request);
}
