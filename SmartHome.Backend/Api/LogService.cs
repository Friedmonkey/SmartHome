using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Api
{
    public class LogService : ILogService
    {
        private readonly ApiContext _ctx;

        public LogService(ApiContext context)
        {
            _ctx = context;
        }

        public Task<SuccessResponse> CreateLog(ILogService.CreateLogRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request)
        {
            List<Log> logsList =  await _ctx.DbContext.Logs.Where(l => l.SmartHomeId == request.smartHome).ToListAsync();

            return new LogListResponse(logsList);
        }
    }
}
