namespace SmartHome.Backend;

public class BackendConfig
{
    private IConfiguration _configuration;

    public BackendConfig(IConfiguration _configurationManager)
    {
        this._configuration = _configurationManager;
    }

    public string GetOption(string name, string? overrideKey = null)
    {
        return _configuration[overrideKey ?? name] ?? throw new NullReferenceException(name);
    }

    public string Domain => GetOption("Domain");
    public string JwtSecret => GetOption("JwtSecret");
    public string FrontenUrl => GetOption("FrontenUrl");
    public string ConnectionString => GetOption("ConnectionString", "ConnectionStrings:DBConnection");

}