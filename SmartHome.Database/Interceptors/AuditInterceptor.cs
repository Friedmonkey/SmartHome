using SmartHome.Database.Entities;

namespace SmartHome.Database.Interceptors;

public class AuditInterceptor() : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext eventDataContext)
    {
        var entities = eventDataContext.ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State is (EntityState.Added or EntityState.Modified))
            .ToList();

        var now = DateTimeOffset.Now;

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(nameof(IAuditable.CreatedOn)).CurrentValue = now;
                entity.Property(nameof(IAuditable.ModifiedOn)).CurrentValue = now;
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Property(nameof(IAuditable.CreatedOn)).IsModified = false;
                entity.Property(nameof(IAuditable.ModifiedOn)).CurrentValue = now;
            }
        }
    }
}