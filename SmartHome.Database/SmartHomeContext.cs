namespace SmartHome.Database;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartHome.Common.Models.Entities;
using SmartHome.Database.Auth;

public class SmartHomeContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<AuthAccount, Role, Guid>
{
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options)
    {
    }
    public DbSet<OldAccount> Accounts { get; set; }
    public DbSet<DeviceAccess> DeviceAccesses { get; set; }
    public DbSet<DeviceAction> DeviceAction { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Routine> Routine { get; set; }
    public DbSet<Home> Home { get; set; }
    public DbSet<SmartUser> SmartUser { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //auth
        modelBuilder.Entity<AuthAccount>().HasKey(aa => aa.Id);
        modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(r => new { r.UserId, r.RoleId });
        modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        //entities
        modelBuilder.Entity<SmartUser>()
           .HasOne(su => su.Account)
           .WithMany()
           .HasForeignKey(su => su.AccountId)
           .OnDelete(DeleteBehavior.Cascade); // Your existing rule

        modelBuilder.Entity<SmartUser>()
            .HasOne(su => su.SmartHome)
            .WithMany()
            .HasForeignKey(su => su.SmartHomeId)
            .OnDelete(DeleteBehavior.Cascade); // Your existing rule

        modelBuilder.Entity<Room>()
           .HasOne(r => r.SmartHome)
           .WithMany()
           .HasForeignKey(r => r.SmartHomeId)
           .OnDelete(DeleteBehavior.Cascade); // Adjust if needed

        modelBuilder.Entity<Log>()
            .HasOne(l => l.SmartHome)
            .WithMany()
            .HasForeignKey(l => l.SmartHomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Device>()
            .HasOne(d => d.Room)
            .WithMany()
            .HasForeignKey(l => l.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Routine>()
            .HasOne(d => d.SmartHome)
            .WithMany()
            .HasForeignKey(l => l.SmartHomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DeviceAccess>()
            .HasOne(d => d.Device)
            .WithMany()
            .HasForeignKey(l => l.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DeviceAccess>()
           .HasOne(d => d.SmartUser)
           .WithMany()
           .HasForeignKey(l => l.SmartUserId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DeviceAction>()
           .HasOne(d => d.Device)
           .WithMany()
           .HasForeignKey(l => l.DeviceId)
           .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<DeviceAction>()
           .HasOne(d => d.Routine)
           .WithMany()
           .HasForeignKey(l => l.RoutineId)
           .OnDelete(DeleteBehavior.Cascade);
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
