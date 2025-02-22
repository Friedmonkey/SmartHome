using SmartHome.Common.Api;
using SmartHome.Common;

namespace SmartHome.UI.Api
{
    public class LogService : ILogService
    {
        private readonly ApiService _api;

        public LogService(ApiService api)
        {
            this._api = api;
        }

        public Task<SuccessResponse> CreateLog(ILogService.CreateLogRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request)
        {
            return await _api.Get<LogListResponse>(SharedConfig.Urls.Log.GetAllLogs, request);
        }
    }
}
