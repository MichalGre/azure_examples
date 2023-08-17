using Microsoft.Identity.Client;

namespace public_app
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Config config = Config.ReadFromJsonFile("appsettings.json");

            var app = PublicClientApplicationBuilder.Create(config.ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.Tenant)
                .WithRedirectUri("http://localhost")
                .Build();

            string[] scopes = { "user.read" };
            var result = app.AcquireTokenInteractive(scopes).ExecuteAsync();
            Console.WriteLine($"Token: {result.Result.AccessToken}");
        }
    }
}
