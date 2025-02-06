using FastEndpoints;
using SmartHome.Common;
using SmartHome.Common.Models;

namespace SmartHome.Backend.FastEndpoints;

public class Weather: Ep.NoReq.Res<IEnumerable<WeatherResponse>>
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    public override void Configure()
    {
        Get(SharedConfig.WeatherUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(Enumerable.Range(1, 5).Select(index => new WeatherResponse
        (   Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC: Random.Shared.Next(-20, 55),
            Summary: Summaries[Random.Shared.Next(Summaries.Length)]
        )).ToArray());
    }

}
