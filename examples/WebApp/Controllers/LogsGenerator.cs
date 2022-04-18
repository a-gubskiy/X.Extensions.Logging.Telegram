using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsGenerator : ControllerBase
{
    private const string LogMessage =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc vitae sem enim. Nam congue non.";

    private readonly ILogger<LogsGenerator> _logger;

    public LogsGenerator(ILogger<LogsGenerator> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int amount)
    {
        while (amount > 0)
        {
            var logLevel = (LogLevel)Random.Shared.NextInt64(0, 7);
            _logger.Log(logLevel, LogMessage);
            
            amount--;
        }
        return Ok();
    }
}