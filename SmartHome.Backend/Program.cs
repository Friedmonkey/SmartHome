public partial class Program { }

using SmartHome.Backend.Auth;

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


        app.MapControllers();

        app.Run();
    }
}
