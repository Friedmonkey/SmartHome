namespace SmartHome.Backend;

public record BackendConfig
{
    public required string Domain { get; init; }
    public required string JwtKey { get; init; }
    public static BackendConfig GetDefaultConfig()
    {
        return new BackendConfig()
        {
            JwtKey = "lol idk",
#if DEBUG
            Domain = "http://localhost:7700",
#else
            Domain = "https://smart.friedmonkey.nl",
#endif
        };
    }
}
