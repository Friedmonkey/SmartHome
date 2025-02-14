using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartHome.Backend.Auth;
using SmartHome.Common.Models;

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

