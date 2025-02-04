namespace SmartHome.Backend.Auth;

public static class CorsSetupExtention
{
    public static void SetupCors(this IServiceCollection services, BackendConfig config)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(config.FrontendCorsKey,
                policy => policy.WithOrigins(config.FrontenUrl)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
            );
        });
    }
}
