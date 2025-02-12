using SmartHome.Database;
using System.Security.Claims;

namespace SmartHome.Backend.Api;

public class ApiContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly SmartHomeContext _dbContext;

    public ApiContext(IHttpContextAccessor contextAccessor, SmartHomeContext dbContext)
    { 
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
    }

    public ClaimsPrincipal? User => _contextAccessor?.HttpContext?.User;
    public SmartHomeContext DbContext => _dbContext;
}
