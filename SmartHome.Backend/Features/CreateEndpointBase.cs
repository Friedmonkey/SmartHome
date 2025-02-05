using FluentValidation;

namespace SmartHome.Backend.Features;

public abstract class CreateEndpointBase<TRequest, TResponse, TEntity, TMapper, TValidator>
    (SmartHomeDbContext SmartHomeDbContext)
    : EndpointBase<TRequest, TResponse, TMapper, TValidator>
    where TRequest : notnull
    where TResponse : notnull
    where TMapper : Mapper<TRequest, TResponse, TEntity>
    where TValidator : AbstractValidator<TRequest>
{
    protected SmartHomeDbContext SmartHomeDbContext { get; set; } = SmartHomeDbContext;

    public override async Task<TResponse> ExecuteAsync(TRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);
        SmartHomeDbContext.Add(entity!);

        await SmartHomeDbContext.SaveChangesAsync(ct);

        return Map.FromEntity(entity);
    }
}