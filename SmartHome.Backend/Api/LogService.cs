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
        public async Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request)
        {
            var result =  await _ctx.DbContext.Logs.Where(l => l.SmartHomeId == request.smartHome).ToListAsync();
            return new LogListResponse(result);
        }

        public async Task<SuccessResponse> CreateLog(LogRequest request)
        {
            var result = await _ctx.DbContext.Logs.AddAsync(request.Log);

            await _ctx.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
    }
}
