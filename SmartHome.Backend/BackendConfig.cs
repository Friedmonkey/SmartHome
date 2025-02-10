namespace SmartHome.Backend;

public class BackendConfig
{
    private ConfigurationManager _configurationManager;

    public BackendConfig(ConfigurationManager _configurationManager)
    {
        this._configurationManager = _configurationManager;
    }

    public string Domain => _configurationManager["Domain"] ?? throw new NullReferenceException(nameof(Domain));
    public string JwtKey => _configurationManager["JwtKey"] ?? throw new NullReferenceException(nameof(JwtKey));
    public string FrontenUrl => _configurationManager["FrontenUrl"] ?? throw new NullReferenceException(nameof(FrontenUrl));
    public string FrontendCorsKey => _configurationManager["FrontendCorsKey"] ?? throw new NullReferenceException(nameof(FrontendCorsKey));
    public string ConnectionString => _configurationManager["ConnectionStrings : SmartHomeDb"] ?? throw new NullReferenceException(nameof(ConnectionString));

}

