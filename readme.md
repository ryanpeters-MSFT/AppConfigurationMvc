# Azure App Configuration MVC
This is a quick example of using the Azure App Configuration SDK and client to plug configuration data into your existing `IConfiguration` instance with minimal changes required to the code. This demo also shows the use of the "pull" method for synching changes to keys from the store using a sentinal key, thus forcing a refresh after a cache timeout period.

Replace the `AppConfigurationConnection` key with your App Configuration store connection string. Optionally, if you have any keys that link to KeyVault, set the `KeyVaultEndpoint` key/value as well. 

In addition, it briefly shows how to use `ConfigurationClient` to manipulate keys/values in your configuration store.