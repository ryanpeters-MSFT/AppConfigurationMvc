using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AppConfigurationMvc.Controllers;

public class HomeController(IConfigurationRefresherProvider refresherProvider, IConfiguration configuration) : Controller
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

        await Task.WhenAll(refreshers.Select(r => r.TryRefreshAsync()));

        return Ok("App configuration refreshed.");
    }
}
