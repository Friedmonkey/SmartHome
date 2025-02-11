using System.Security.Claims;

namespace SmartHome.Backend.Api;

public class ApiContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    public ApiContext(IHttpContextAccessor contextAccessor)
    { 
        _contextAccessor = contextAccessor;
    }

    public ClaimsPrincipal? User => _contextAccessor?.HttpContext?.User;
}
