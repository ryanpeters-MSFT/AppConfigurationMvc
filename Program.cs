using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var environment = "development";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration["AppConfigurationConnection"];

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connection);

    // load all settings in addition to those with the label matching the environment
    options.Select("*").Select("*", environment);

    // or, select using a filter
    //options.Select("*").Select("Apis/*", environment);

    options.ConfigureRefresh(options => 
    {
        // force this setting to only refresh every 20 seconds
        options.SetCacheExpiration(TimeSpan.FromSeconds(20));
        
        // trigger a refresh based on a change to this key
        // refreshAll: true will changed ALL keys when the value of THIS key is changed
        // https://learn.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-aspnet-core?tabs=core6x#add-a-sentinel-key
        options.Register("TestSetting", refreshAll: true);
    });

    options.ConfigureKeyVault(options => 
    {
        // use an environment variable for this
        var keyVaultEndpoint = builder.Configuration["KeyVaultEndpoint"];

        // for using keyvault, a proper credential is required
        options.SetCredential(new DefaultAzureCredential());

        //options.SetSecretRefreshInterval(TimeSpan.FromSeconds(60));
    });
});

// add supporting services to allow middleware to handle interval refresh
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

app.UseAzureAppConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
