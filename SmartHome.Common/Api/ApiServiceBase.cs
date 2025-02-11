using System.Security.Claims;

namespace SmartHome.Common.Api;

//public class ApiServiceBase
//{
//    public Func<ApiContext>? _getContextCallback = null;
//    public ApiContext GetApiContext() => _getContextCallback?.Invoke() ?? throw new NotImplementedException();
//    public ClaimsPrincipal ApiUser => GetApiContext().User;
//}
//public record ApiContext(ClaimsPrincipal User);