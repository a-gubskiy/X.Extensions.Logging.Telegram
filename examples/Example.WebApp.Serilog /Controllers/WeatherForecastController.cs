using Microsoft.AspNetCore.Mvc;

namespace Example.WebApp.Serilog_.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        var temperatureC = rng.Next(-20, 55);
            
        _logger.LogTrace($"Trace Temperature: {temperatureC}");
        _logger.LogDebug($"Debug Temperature: {temperatureC}");
        _logger.LogInformation($"Information Temperature: {temperatureC}");
        _logger.LogWarning($"Warning Temperature: {temperatureC}");
        _logger.LogError($"Error Temperature: {temperatureC}");
        _logger.LogCritical($"Critical Temperature: {temperatureC}");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temperatureC,
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
    }
}