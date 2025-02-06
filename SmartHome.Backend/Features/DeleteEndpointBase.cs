using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Commands.Base;

namespace SmartHome.Backend.Features;

public abstract class DeleteEndpointBase<TRequest, TEntity, TValidator>
    (SmartHomeDbContext SmartHomeDbContext)
    : EndpointBase<TRequest, Results<Ok, NotFound>, TValidator>
    where TRequest : notnull, IDeleteCommand
    where TEntity : class, IEntityBase
    where TValidator : AbstractValidator<TRequest>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(TRequest req, CancellationToken ct)
    {
        var entityItem = await SmartHomeDbContext
            .Set<TEntity>()
            .SingleOrDefaultAsync(x => x.Id == req.Id);

        if (entityItem is null)
        {
            return NotFound();
        }

        SmartHomeDbContext.Remove(entityItem);
        await SmartHomeDbContext.SaveChangesAsync(ct);

        return Ok();
    }
}