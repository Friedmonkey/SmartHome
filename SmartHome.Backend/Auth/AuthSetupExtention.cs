using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SmartHome.Backend.Auth;

public static class AuthSetupExtention
{
    public static void SetupJWTAuthServices(this IServiceCollection services, BackendConfig config)
    {
        services.AddDbContextFactory<AuthContext>(options =>
        {
            options.UseSqlite($"Data Source=database.db");
        });
        services.AddScoped(p =>
        {
            var context = p.GetRequiredService<IDbContextFactory<AuthContext>>().CreateDbContext();
            context.Database.EnsureCreated();

            return context;
        });
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = config.Domain,
                ValidateIssuer = false,
                ValidIssuer = config.Domain,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.JwtKey)),
            };
        });

        services.AddAuthorization();

        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
.AddEntityFrameworkStores<AuthContext>()
.AddDefaultTokenProviders();

    }
    public static void SetupJWTAuthApp(this WebApplication app, BackendConfig config)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
