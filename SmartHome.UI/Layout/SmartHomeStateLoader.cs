using Microsoft.AspNetCore.Components;
using MudBlazor;
using SmartHome.Common.Api;
using SmartHome.UI.Api;

namespace SmartHome.UI.Layout;

public class SmartHomeStateLoader
{
    private readonly SmartHomeState _smartHomeState;
    private readonly NavigationManager _navManager;
    private readonly ISmartHomeService _smartHomeService;
    private readonly ISnackbar _snackBar;
    public SmartHomeStateLoader(SmartHomeState smartHomeState, NavigationManager navManager, ISmartHomeService smartHomeService, ISnackbar snackBar)
    {
        _smartHomeState = smartHomeState;
        _navManager = navManager;
        _smartHomeService = smartHomeService;
        _snackBar = snackBar;
    }

    public async Task<bool> ResolveSmartHomeState(string? smartHomeGuidStr)
    {
        if (Guid.TryParse(smartHomeGuidStr, out Guid smartHomeId))
        {
            if (_smartHomeState.SelectedSmartHomeId.HasValue &&
                _smartHomeState.SelectedSmartHomeId == smartHomeId)
                return true; //we do not need to get the same one.

            var request = new GuidRequest((Guid)smartHomeId);
            var smartHomeResponse = await _smartHomeService.GetSmartHomeById(request);

            if (smartHomeResponse.EnsureSuccess(_snackBar)) //only logs errors
            {
#if DEBUG
                _snackBar.Add("Smarthome retrived!", Severity.Info); //let devs know we made api call
#endif
                _smartHomeState.SetSelectedSmartHome(smartHomeResponse.smartHome);
                return true;
            }
        }

        _smartHomeState.SetSelectedSmartHome(null);
        _navManager.NavigateTo("/smarthome");
        return false;
    }
}
