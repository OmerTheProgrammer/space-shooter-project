using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_Manager___API;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// In WASM, we usually pull the config from the browser's environment or a hardcoded fallback
string serverUrl = builder.Configuration.GetConnectionString("SpaceShooterServer") ?? "";

// Register the same service for the Browser side
builder.Services.AddScoped<IApiService>(sp => new ApiService(serverUrl));

await builder.Build().RunAsync();