using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_Manager___API;
using Space_Shooter_Website.Client.Support_Classes;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the same service for the Browser side
builder.Services.AddScoped<IApiService>(sp => new ApiService());
builder.Services.AddScoped<Session>();

await builder.Build().RunAsync();