using SmartHome.Common.Api;

namespace SmartHome.Backend.Api
{
    public class LogService : ILogService
    {
        private readonly ApiContext _ctx;

        public LogService(ApiContext context)
        {
            _ctx = context;
        }
        public async Task<SuccessResponse> GetAllLogs(EmptySmartHomeRequest request)
        {


            return SuccessResponse.Success();
        }
    }
}
