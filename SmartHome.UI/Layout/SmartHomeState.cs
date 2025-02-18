using Microsoft.AspNetCore.Components;

namespace SmartHome.UI.Layout;

public class SmartHomeState
{
    private readonly NavigationManager _navigationManager;
    public SmartHomeState(NavigationManager navigationManager)
    {
        this._navigationManager = navigationManager;
    }
    public Guid? GetCurrentSmartHomeGuid()
    {
        var guidStr = GetCurrentSmartHomeGuidStr();
        if (guidStr is null)
            return null;
        if (Guid.TryParse(guidStr, out Guid guid))
            return guid;
        else
            return null;
    }

    public string? GetCurrentSmartHomeGuidStr()
    {
        var uri = new Uri(_navigationManager.Uri);
        if (uri.Segments.Count() < 3) // smarthome/{guid}/page
            return null;

        if (!uri.Segments[^3].ToLower().EndsWith("smarthome/"))
            return null;

        var guidString = uri.Segments[^2].Replace("/", "");
        return guidString;
    }
}

