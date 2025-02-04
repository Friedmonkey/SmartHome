namespace SmartHome.UI;

public record FrontendConfig
{
    public required string ApiBaseUrl { get; init; }
    public required string HttpClientName { get; init; }
    public static FrontendConfig GetDefaultConfig() 
    {
        return new FrontendConfig()
        {
            HttpClientName = "MyApp.ServerAPI",
#if DEBUG
            ApiBaseUrl = "",
#else
            ApiBaseUrl = "",
#endif
        };
    }
}
