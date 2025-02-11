using FastEndpoints;
//using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Api;
using SmartHome.Backend.Auth;
using SmartHome.Common.Api;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using FastEndpoints.Swagger;
//using Microsoft.EntityFrameworkCore;

namespace SmartHome.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new BackendConfig(builder.Configuration);
        builder.Services.AddSingleton<BackendConfig>(config);

        // Add services to the container.
        //database for now, needs to be replaced
        builder.Services.AddDbContextFactory<AuthContext>(options =>
        {
            options.UseSqlite($"Data Source=database.db");
        });
        builder.Services.AddScoped(p =>
        {
            var context = p.GetRequiredService<IDbContextFactory<AuthContext>>().CreateDbContext();
            context.Database.EnsureCreated();

            return context;
        });

        //jwt auth
        builder.Services.AddAuthenticationJwtBearer(options => options.SigningKey = config.JwtSecret);
        builder.Services.AddAuthorization();

        //setup user in database
        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<AuthContext>()
        .AddDefaultTokenProviders();

        //builder.Services.SetupJWTAuthServices(config);

        //builder.Services.AddScoped<IDatabase, MemoryDatabase>();

        //add our services
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IPersonTestingService, PersonTestingService>();

        builder.Services.AddFastEndpoints();
        builder.Services.SwaggerDocument();

        //builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();

        //allow the fontend to access us
        const string corsKey = "SmartHomeAllowFrontendCors";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsKey,
                policy => policy.WithOrigins(config.FrontenUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
        });

        var app = builder.Build();
        app.UseCors(corsKey);

        app.UseAuthentication();
        app.UseAuthorization();
        //app.SetupJWTAuthApp(config);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
        }


        app.UseHttpsRedirection();

        //app.UseAuthorization();

        app.UseFastEndpoints();
        app.UseSwaggerGen();

        //app.MapControllers();
        //System.Diagnostics.Process.Start("cmd", "/c start https://localhost:5200/swagger/index.html");
        app.Run();
    }
}
