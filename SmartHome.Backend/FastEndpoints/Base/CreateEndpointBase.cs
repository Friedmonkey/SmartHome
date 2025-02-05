using FastEndpoints;
using FluentValidation;
using SmartHome.Backend.Auth;

namespace SmartHome.Backend.FastEndpoints.Base;

public abstract class CreateEndpointBase<TRequest, TResponse, TEntity, TMapper, TValidator>
    (AuthContext SmartHomeDbContext)
    : EndpointBase<TRequest, TResponse, TMapper, TValidator>
    where TRequest : notnull
    where TResponse : notnull
    where TMapper : Mapper<TRequest, TResponse, TEntity>
    where TValidator : AbstractValidator<TRequest>
{
    protected AuthContext SmartHomeDbContext { get; set; } = SmartHomeDbContext;
    public override async Task<TResponse> ExecuteAsync(TRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);
        SmartHomeDbContext.Add(entity!);
        await SmartHomeDbContext.SaveChangesAsync(ct);
        return Map.FromEntity(entity);
    }
}
