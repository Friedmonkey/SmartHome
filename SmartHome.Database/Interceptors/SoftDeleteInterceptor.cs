using SmartHome.Database.Entities;

namespace SmartHome.Database.Interceptors;

public class SoftDeleteInterceptor() : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            SoftDeleteEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SoftDeleteEntities(DbContext eventDataContext)
    {
        var entities = eventDataContext.ChangeTracker.Entries<IDeletable>()
            .Where(e => e.State is EntityState.Deleted)
            .ToList();

        var now = DateTimeOffset.Now;

        foreach (var entity in entities)
        {
            entity.State = EntityState.Modified;
            entity.Properties.ForEach(x => x.IsModified = false);
            entity.Property(nameof(IDeletable.DeletedOn)).CurrentValue = now;
        }
    }
}