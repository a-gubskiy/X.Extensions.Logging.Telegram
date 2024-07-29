namespace Example.Core.Models;

public record WeatherForecast
{
    public int TemperatureC { get; init; }
    public string? Summary { get; init; }
    public DateTime Date { get; init; }
}