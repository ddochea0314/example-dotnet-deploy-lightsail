using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherApi.Controllers;

namespace WeatherApi.Test;

public class WeatherForecastControllerTest
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastControllerTest()
    {
        _logger = new Mock<ILogger<WeatherForecastController>>().Object;
    }

    [Fact]
    public void GetWeatherForecast_when_count_5Async()
    {
        // Arrange
        var controller = new WeatherForecastController(_logger);

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Count());
    }
}