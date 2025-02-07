namespace SmartHome.Common.Models;

public record WeatherResponse(WeatherForecast[] Forcasts) : Response<WeatherResponse>;

public record WeatherForecast(DateOnly Date, float TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
