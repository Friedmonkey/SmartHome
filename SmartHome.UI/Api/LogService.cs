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

        public async Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request)
        {
            return await _api.Get<LogListResponse>(SharedConfig.Urls.Log.GetAllLogs, request);
        }

        public async Task<SuccessResponse> CreateLog(LogRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Log.CreateLog, request);
        }
    }
}
