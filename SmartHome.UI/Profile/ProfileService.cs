using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace SmartHome.UI.Profile;
public class ProfileService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IJSRuntime _jsRuntime;

    public event Action<Preferences>? OnChange;

    public ProfileService(ILocalStorageService localStorageService, IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _localStorageService = localStorageService;
    }

    public async Task ToggleDarkMode()
    {
        var preferences = await GetPreferences();
        var newPreferences = preferences with //Record "copy" constructor
        { 
            DarkMode = !preferences.DarkMode
        };
        await _localStorageService.SetItemAsync(preferences_key, newPreferences);
        OnChange?.Invoke(newPreferences);
    }

    private const string preferences_key = "preferences";
    public async Task<Preferences> GetPreferences()
    {
        if (await _localStorageService.ContainKeyAsync(preferences_key))
            return (await _localStorageService.GetItemAsync<Preferences>(preferences_key))!;

        var prefersDarkMode = await _jsRuntime.InvokeAsync<bool>("prefersDarkMode");
        return new Preferences
        {
            DarkMode = prefersDarkMode
        };
    }

}