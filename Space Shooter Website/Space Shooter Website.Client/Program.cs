using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_Manager___API;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the same service for the Browser side
builder.Services.AddScoped<IApiService>(sp => new ApiService());

await builder.Build().RunAsync();