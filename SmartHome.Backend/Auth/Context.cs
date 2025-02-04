using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.Backend.Auth;

public class AuthContext : IdentityDbContext<User, Role, long>
{
    public AuthContext(DbContextOptions options) : base(options)
    {
    }
}
