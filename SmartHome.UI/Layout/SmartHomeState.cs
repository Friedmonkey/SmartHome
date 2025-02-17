using SmartHome.Common.Models.Entities;

namespace SmartHome.UI.Layout;

public class SmartHomeState
{
    private SmartHomeModel? _selectedSmartHome;
    //if you want this to load make sure to call await base.OnInitializedAsync(); in your own OnInitializedAsync

    public event Action? OnChange;

    public void SetSelectedSmartHome(SmartHomeModel? smartHome)
    {
        _selectedSmartHome = smartHome;
        OnChange?.Invoke();
    }

    public Guid? SelectedSmartHomeId => _selectedSmartHome?.Id;
    public string SelectedSmartHomeName => _selectedSmartHome?.Name ?? "No Home selected";
}

