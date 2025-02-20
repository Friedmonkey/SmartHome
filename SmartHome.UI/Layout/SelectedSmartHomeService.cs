using Microsoft.AspNetCore.Components;

namespace SmartHome.UI.Layout;

public class SelectedSmartHomeService
{
    private readonly NavigationManager _navigationManager;
    public SelectedSmartHomeService(NavigationManager navigationManager)
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
        var count = uri.Segments.Count();
        if (count < 3) // smarthome/{guid}/page
            return null;
        int idx = uri.Segments.ToList().FindIndex(0, count, str => str.ToLower().EndsWith("smarthome/"));
        if (idx == -1 || idx == 0)
            return null;

        if (count < idx + 1)
            return null;

        var guidString = uri.Segments[idx + 1].Replace("/", "");
        return guidString;
    }

    public string GetUrlWithDiffrentSmartHomeId(Guid newId)
    {
        var segments = GetCurrentUriSegmentsAfterSmartHome().Skip(2).Select(ss => ss.Trim('/')).ToList();
        List<string> newUrl = new();
        newUrl.Add("smarthome");
        newUrl.Add(newId.ToString());
        newUrl.AddRange(segments);
        return string.Join('/', newUrl);
    }
    public List<string> GetCurrentUriSegmentsAfterSmartHome()
    {
        List<string> segments = new();
        var uri = new Uri(_navigationManager.Uri);
        var count = uri.Segments.Count();

        if (count < 3) // smarthome/{guid}/page
            return segments;

        int idx = uri.Segments.ToList().FindIndex(0, count, str => str.ToLower().EndsWith("smarthome/"));
        if (idx == -1)
            return segments;

        segments = uri.Segments.Skip(idx).ToList();
        return segments;
    }
}

