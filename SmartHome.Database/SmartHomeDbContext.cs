namespace SmartHome.Database;

public class SmartHomeDbContext
    : DbContext
{

    public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options)
        : base(options)
    {}

    public DbSet<Account>? Accounts { get; set; }

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
}