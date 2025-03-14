using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Api;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using FastEndpoints.Swagger;
using SmartHome.Database;
using SmartHome.Database.Auth;
using SmartHome.Common.Api;
using SmartHome.Database.ApiContext;

namespace SmartHome.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new BackendConfig(builder.Configuration);
        builder.Services.AddSingleton<BackendConfig>(config);

        // Add services to the container.
        builder.Services.AddDbContext<SmartHomeContext>(options => options.UseMySql(config.ConnectionString, ServerVersion.AutoDetect(config.ConnectionString)));

        //jwt auth
        builder.Services.AddAuthenticationJwtBearer(options => options.SigningKey = config.JwtSecret);
        builder.Services.AddAuthorization();

        //setup user in database
        builder.Services.AddIdentity<AuthAccount, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<SmartHomeContext>()
        .AddDefaultTokenProviders();

        //builder.Services.SetupJWTAuthServices(config);

        //builder.Services.AddScoped<IDatabase, MemoryDatabase>();

        //add our services

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddScoped<DeviceContext>();
        builder.Services.AddScoped<AuthContext>();
        builder.Services.AddScoped<RoomContext>();
        builder.Services.AddScoped<RoutineContext>();
        builder.Services.AddScoped<ApiContext>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<ILogService, LogService>();
        builder.Services.AddScoped<IRoutineService, RoutineService>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<ISmartHomeService, SmartHomeService>();

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
