
using System.Net.Http.Headers;
using app;
using Microsoft.Graph;
using Microsoft.Identity.Client;

internal class Program
{
   private static void Main(string[] args)
   {
      Console.WriteLine("Hello, World!");

      Config config = Config.ReadFromJsonFile("appsettings.json");

      IConfidentialClientApplication app;

      app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                 .WithClientSecret(config.ClientSecret)
                 .WithAuthority(new Uri(config.Authority))
                 .Build();

      string[] scopes = new string[] { $"{config.ApiUrl}.default" }; // Generates a scope -> "https://graph.microsoft.com/.default"

      callGraphApiAsync(app, scopes).GetAwaiter().GetResult();

   }

   private static async Task callGraphApiAsync(IConfidentialClientApplication app, string[] scopes)
   {
      GraphServiceClient gsc = new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
           {

              AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                  .ExecuteAsync();

              requestMessage.Headers.Authorization =
                  new AuthenticationHeaderValue("Bearer", result.AccessToken);

           }));

      try
      {
         var users = await gsc.Users.Request().GetAsync();
         Console.WriteLine($"Number of users: {users.Count()}");
      }
      catch (Exception ex)
      {
         Console.WriteLine($"{ex.Message}");
      }
   }
}