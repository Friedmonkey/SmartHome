using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartHome.Common.Models;
using SmartHome.Database.Auth;

namespace SmartHome.Backend.FastEndpoints.Base;

public abstract class BasicEndpointBase<TRequest, TResponse>: Endpoint<TRequest, TResponse>
    where TRequest: notnull
    where TResponse: Response<TResponse>
{
    public void SecureJwtEndpoint()
    {
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles(AuthRoles.AuthUser);
    }
}
