namespace SmartHome.Backend;

public class BackendConfig
{
    private ConfigurationManager _configurationManager;

    public BackendConfig(ConfigurationManager _configurationManager)
    {
        this._configurationManager = _configurationManager;
    }

    public string GetOption(string name, string? overrideKey = null)
    {
        return _configurationManager[overrideKey ?? name] ?? throw new NullReferenceException(name);
    }

    public string Domain => GetOption("Domain");
    public string JwtKey => GetOption("JwtKey");
    public string FrontenUrl => GetOption("FrontenUrl");
    public string ConnectionString => GetOption("ConnectionString", "ConnectionStrings : SmartHomeDb");

}