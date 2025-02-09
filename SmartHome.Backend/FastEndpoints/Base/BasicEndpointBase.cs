using FastEndpoints;
using SmartHome.Common.Models;

namespace SmartHome.Backend.FastEndpoints.Base;

public abstract class BasicEndpointBase<TRequest, TResponse>: Endpoint<TRequest, TResponse>
    where TRequest: notnull
    where TResponse: Response<TResponse>
{
}
