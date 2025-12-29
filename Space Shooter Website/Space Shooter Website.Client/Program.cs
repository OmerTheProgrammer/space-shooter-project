using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_Manager___API;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// we pull the config from a file
string serverUrl = builder.Configuration.GetConnectionString("SpaceShooterServer") ?? "";

// Register the same service for the Browser side
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(serverUrl);
    // This allows the browser to bypass the tunnel warning screen
    client.DefaultRequestHeaders.Add("X-Tunnel-Skip-AntiPhishing-Scan", "true");
});

await builder.Build().RunAsync();