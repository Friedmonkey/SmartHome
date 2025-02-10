using SmartHome.Backend.Auth;

namespace SmartHome.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new BackendConfig(builder.Configuration);

        // Add services to the container.
        builder.Services.AddSingleton(config);
        builder.Services.SetupJWTAuthServices(config);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.SetupCors(config);
        builder.Services.AddSqlServer<SmartHomeDbContext>(builder.Configuration["SmartHomeDb"]);

        builder.Services.AddFastEndpoints();

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

        app.MapControllers();

        app.Run();
    }
}
