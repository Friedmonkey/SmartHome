namespace SmartHome.Backend;

public record BackendConfig
{
    public required string Domain { get; init; }
    public required string JwtKey { get; init; }
    public required string FrontenUrl { get; init; }
    public required string FrontendCorsKey { get; init; }
    public static BackendConfig GetDefaultConfig()
    {
        return new BackendConfig()
        {
#if DEBUG
            JwtKey = "lol idk",
            Domain = "http://localhost:7700",
            FrontenUrl = "https://localhost:7126",
            FrontendCorsKey = "AllowFrontendDevelopment",
#else
            JwtKey = "sooooo verry secuere!11!",
            Domain = "https://smart.friedmonkey.nl",
            FrontenUrl = "https://smart-api.friedmonkey.nl",
            FrontendCorsKey = "AllowFrontendProduction",
#endif
        };
    }
}
