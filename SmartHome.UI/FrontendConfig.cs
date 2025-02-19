using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace SmartHome.UI;

public class FrontendConfig
{
    private WebAssemblyHostConfiguration _configurationManager;

    public FrontendConfig(WebAssemblyHostConfiguration _configurationManager)
    {
        this._configurationManager = _configurationManager;
    }

    public string GetOption(string name, string? overrideKey = null)
    {
        return _configurationManager[overrideKey ?? name] ?? throw new NullReferenceException(name);
    }
    public string ApiBaseUrl => GetOption("ApiBaseUrl");
    public string HttpClientName => GetOption("HttpClientName");
}
