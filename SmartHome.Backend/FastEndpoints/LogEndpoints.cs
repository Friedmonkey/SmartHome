using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common.Api;
using SmartHome.Common;

namespace SmartHome.Backend.FastEndpoints
{
    public class LogEndpoints
    {
        public class GetAllLogsEndpoint : BasicEndpointBase<EmptySmartHomeRequest, LogListResponse>
        {
            public required ILogService Service { get; set; }
            public override void Configure()
            {
                Get(SharedConfig.Urls.Log.GetAllLogs);
                SecureJwtEndpoint();
            }

            public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
            {
                try
                {
                    await SendAsync(await Service.GetAllLogs(request));
                }
                catch (Exception ex)
                {
                    await SendAsync(LogListResponse.Error(ex));
                }
            }
        }

        public class CreateLogEndpoint : BasicEndpointBase<LogRequest, SuccessResponse>
        {
            public required ILogService Service { get; set; }
            public override void Configure()
            {
                Post(SharedConfig.Urls.Log.CreateLog);
                SecureJwtEndpoint();
            }

            public override async Task HandleAsync(LogRequest request, CancellationToken ct)
            {
                try
                {
                    await SendAsync(await Service.CreateLog(request));
                }
                catch (Exception ex)
                {
                    await SendAsync(SuccessResponse.Error(ex));
                }
            }
        }
    }
}
