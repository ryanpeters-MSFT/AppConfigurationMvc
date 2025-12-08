using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AppConfigurationMvc.Controllers;

public class HomeController(IConfigurationRefresherProvider refresherProvider, IConfiguration configuration, ILogger<HomeController> logger) : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        ViewBag.Configuration = configuration;

        return View();
    }

    [HttpGet("/refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        var refreshers = refresherProvider.Refreshers;

        foreach (var refresher in refreshers)
        {
            logger.LogInformation("Refreshing configuration from {0}...", refresher.AppConfigurationEndpoint);
        }

        await Task.WhenAll(refreshers.Select(r => r.TryRefreshAsync()));

        return Ok("App configuration refreshed.");
    }
}
