using Microsoft.Extensions.Options;
using SmartHome.Backend.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
        builder.Services.AddSwaggerGen();

        builder.Services.SetupCors(config);

        var ConnectionString = builder.Configuration.GetConnectionString("DBConnection");
        builder.Services.AddDbContext<SmartHomeContext>(options => options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

        builder.Services.AddFastEndpoints();

        var app = builder.Build();
        app.UseCors(config.FrontendCorsKey);

        app.SetupJWTAuthApp(config);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseFastEndpoints();

        app.MapControllers();

        app.Run();
    }
}
