using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AppConfigurationMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index([FromServices] IConfiguration configuration)
    {
        ViewBag.Configuration = configuration;

        return View();
    }
}
