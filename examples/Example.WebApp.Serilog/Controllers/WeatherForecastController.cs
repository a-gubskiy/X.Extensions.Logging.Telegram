using Example.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApp.Serilog.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        var temperatureC = rng.Next(-20, 55);
            
        logger.LogTrace($"Trace Temperature: {temperatureC}");
        logger.LogDebug($"Debug Temperature: {temperatureC}");
        logger.LogInformation($"Information Temperature: {temperatureC}");
        logger.LogWarning($"Warning Temperature: {temperatureC}");
        logger.LogError($"Error Temperature: {temperatureC}");
        logger.LogCritical($"Critical Temperature: {temperatureC}");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temperatureC,
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
    }
}