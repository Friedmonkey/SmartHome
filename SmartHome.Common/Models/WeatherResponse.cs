namespace SmartHome.Common.Models;

public record WeatherResponse(DateOnly Date, float TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
