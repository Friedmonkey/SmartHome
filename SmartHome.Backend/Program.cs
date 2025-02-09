using FastEndpoints;
using FastEndpoints.Swagger;
using SmartHome.Backend.Api;
using SmartHome.Backend.Auth;
using SmartHome.Common.Api;
using SmartHome.Common.Models;

namespace SmartHome.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = BackendConfig.GetDefaultConfig();


        // Add services to the container.
        builder.Services.AddSingleton(config);

        builder.Services.SetupJWTAuthServices(config);

        //builder.Services.AddScoped<IDatabase, MemoryDatabase>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddFastEndpoints().SwaggerDocument();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.SetupCors(config);

        var app = builder.Build();
        app.UseCors(config.FrontendCorsKey);

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
