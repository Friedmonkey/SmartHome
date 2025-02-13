namespace SmartHome.UI.Layout;

public class SmartHomeState
{
    public Guid? SelectedSmartHomeId { get; private set; }

    public event Action? OnChange;

    public void SetSelectedSmartHome(Guid? smartHomeId)
    {
        SelectedSmartHomeId = smartHomeId;
        OnChange?.Invoke();
    }
}

