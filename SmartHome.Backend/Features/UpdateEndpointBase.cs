using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Commands.Base;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Backend.Features;

public abstract class UpdateEndpointBase<TRequest, TResponse, TEntity, TMapper, TValidator>
    (SmartHomeContext SmartHomeDbContext)
    : EndpointBase<TRequest, Results<Ok<TResponse>, NotFound>, TMapper, TValidator>
    where TRequest : notnull, IUpdateCommand
    where TResponse : notnull
    where TEntity : class, IEntityBase
    where TMapper : Mapper<TRequest, TResponse, TEntity>
    where TValidator : AbstractValidator<TRequest>
{
    public override async Task<Results<Ok<TResponse>, NotFound>> ExecuteAsync(TRequest req, CancellationToken ct)
    {
        var entityItem = SmartHomeDbContext
            .Set<TEntity>()
            .AsTracking()
            .SingleOrDefault(x => x.Id == req.Id);

        if (entityItem is null)
        {
            return NotFound();
        }

        Map.UpdateEntity(req, entityItem);

        var entry = SmartHomeDbContext.Entry(entityItem);

        var modifiedProperties = entry.Properties
            .Where(p => p.IsModified)
            .ToList();

        if (modifiedProperties.Count == 0)
        {
            return Ok(Map.FromEntity(entityItem));
        }

         await SmartHomeDbContext.SaveChangesAsync(ct);

        return Ok(Map.FromEntity(entityItem));
    }
}