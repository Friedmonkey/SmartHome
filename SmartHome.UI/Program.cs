using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartHome.UI.Api;
using MudBlazor.Services;
using MudExtensions.Services;
using SmartHome.Common.Api;
using Blazored.SessionStorage;
using SmartHome.UI.Auth;
using Blazored.LocalStorage;
using SmartHome.UI.Profile;

namespace SmartHome.UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMudServices();
        builder.Services.AddMudExtensions();

        var config = new FrontendConfig(builder.Configuration);
        builder.Services.AddSingleton<FrontendConfig>(config);

        builder.Services.AddHttpClient();
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(config.HttpClientName));


        builder.Services.AddBlazoredSessionStorage();
        builder.Services.AddScoped<IJwtStoreService, JwtSessionStorageService>();
        
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<ProfileService>();

        builder.Services.AddScoped<JwtAuthStateProvider>(); //we need it directly for the apiservice
        builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthStateProvider>()); //very fucking important otherwise auth will desync
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped<ApiService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IPersonTestingService, PersonTestingService>();
        builder.Services.AddScoped<ISmartHomeService, SmartHomeService>();

        var application = builder.Build();
        await application.RunAsync();
    }
}
