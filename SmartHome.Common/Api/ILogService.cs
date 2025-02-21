using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Api
{
    public record LogListResponse(List<Log> Logs) : Response<LogListResponse>;

    public interface ILogService
    {
        
        Task<LogListResponse> GetAllLogs(EmptySmartHomeRequest request);
    }
}
