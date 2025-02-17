using SmartHome.Common.Models.Entities;

namespace SmartHome.UI.Layout;

public class SmartHomeState
{
    private SmartHomeModel? _selectedSmartHome;
    private string? preloadGuid;
    //if you want this to load make sure to call await base.OnInitializedAsync(); in your own OnInitializedAsync

    public event Action? OnChange;

    public void _SetPreloadSelectedSmartHome(string? smartHomeId)
    { 
        preloadGuid = smartHomeId;
    }
    public string? _GetPreloadGuid() => preloadGuid;

    public void SetSelectedSmartHome(SmartHomeModel? smartHome)
    {
        _selectedSmartHome = smartHome;
        OnChange?.Invoke();
    }

    public Guid? SelectedSmartHomeId => _selectedSmartHome?.Id;
    public string SelectedSmartHomeName => _selectedSmartHome?.Name ?? "Select or create a SmartHome.";
}

