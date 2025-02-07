using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartHome.UI.Api;
using SmartHome.UI.Auth;
using MudBlazor.Services;

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

        var config = FrontendConfig.GetDefaultConfig();

        builder.Services.AddSingleton(config);

        builder.Services.AddSingleton<JwtAuthenticationStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());

        var appUri = new Uri(config.ApiBaseUrl);// builder.HostEnvironment.BaseAddress);
        builder.Services.AddScoped(provider => new JwtTokenMessageHandler(appUri, provider.GetRequiredService<JwtAuthenticationStateProvider>()));
        
        
        builder.Services.AddHttpClient(config.HttpClientName, client => client.BaseAddress = appUri)
            .AddHttpMessageHandler<JwtTokenMessageHandler>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(config.HttpClientName));

        builder.Services.AddScoped<ApiService>();

        var application = builder.Build();
        //await RefreshJwtToken(application);

        await application.RunAsync();
    }
    //private static async Task RefreshJwtToken(WebAssemblyHost application)
    //{
    //    using var boostrapScope = application.Services.CreateScope();
    //    var api = boostrapScope.ServiceProvider.GetRequiredService<ApiService>();

    //    var refreshTokenResponse = await api.RefreshToken();
    //    if (refreshTokenResponse.IsSuccess)
    //    {
    //        var loginStateService = boostrapScope.ServiceProvider.GetRequiredService<JwtAuthenticationStateProvider>();
    //        loginStateService.Login(refreshTokenResponse.Token);
    //    }
    //}
}
