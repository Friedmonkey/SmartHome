using SmartHome.Database.Entities;
using SmartHome.Database.Interceptors;
namespace SmartHome.Database;

public class SmartHomeDbContext
    : DbContext
{
    private readonly bool _migrationMode;

    public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options, bool migrationMode = false)
        : base(options)
    {
        _migrationMode = migrationMode;
    }

    public DbSet<User>? Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartHomeDbContext).Assembly);

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
             .SelectMany(t => t.GetForeignKeys())
             .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        if (!_migrationMode)
        {
            optionsBuilder.AddInterceptors(new AuditInterceptor());
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }
    }
}