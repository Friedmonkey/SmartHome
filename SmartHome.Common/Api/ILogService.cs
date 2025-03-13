using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Common.Api;

public record LogListResponse(List<Log> Logs) : Response<LogListResponse>;

public record LogRequest(Log Log) : SmartHomeRequest;

public interface ILogService
{
    Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request);

    public record CreateLogRequest(string Action, string Type) : SmartHomeRequest;
    Task<SuccessResponse> CreateLog(LogRequest request);
}
