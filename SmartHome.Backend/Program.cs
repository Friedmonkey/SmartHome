using FastEndpoints;
using FastEndpoints.Swagger;
using SmartHome.Backend.Api;
using SmartHome.Backend.Auth;
using SmartHome.Common.Api;

namespace SmartHome.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new BackendConfig(builder.Configuration);
        builder.Services.AddSingleton<BackendConfig>(config);

        // Add services to the container.

        builder.Services.SetupJWTAuthServices(config);

        //builder.Services.AddScoped<IDatabase, MemoryDatabase>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IPersonTestingService, PersonTestingService>();

        builder.Services.AddFastEndpoints().SwaggerDocument();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

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

        app.SetupJWTAuthApp(config);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseFastEndpoints();
        app.UseSwaggerGen();

        app.MapControllers();

        app.Run();
    }
}
