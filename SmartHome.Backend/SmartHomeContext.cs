using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class SmartHomeContext: DbContext
{

    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options)
    {

    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<DeviceAccess> DeviceAccesses { get; set; }
    public DbSet<DeviceAction> DeviceAction { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Routine> Routine { get; set; }
    public DbSet<Home> Home { get; set; }
    public DbSet<SmartUser> SmartUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartHomeContext).Assembly);

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
             .SelectMany(t => t.GetForeignKeys())
             .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }
}

public class YourDbContextFactiory : IDesignTimeDbContextFactory<SmartHomeContext>
{

    public SmartHomeContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SmartHomeContext>();

        var ConnectionString = "server=localhost;database=SmartHome;uid=root";
        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

        return new SmartHomeContext(optionsBuilder.Options);
    }
}