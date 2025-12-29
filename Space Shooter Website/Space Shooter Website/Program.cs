using Space_Shooter_Website.Client.Pages;
using Space_Shooter_Website.Components;
using Client_Manager___API;

var builder = WebApplication.CreateBuilder(args);

// 1. Get the URL from appsettings
string serverUrl = builder.Configuration.GetConnectionString("SpaceShooterServer") ?? "";

// 2. Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 3. Register your API Service for the Server (Prerendering)
builder.Services.AddHttpClient<IApiService, ApiService>(client => {
    client.BaseAddress = new Uri(serverUrl); 
    client.DefaultRequestHeaders.Add(
        "X-Tunnel-Skip-AntiPhishing-Scan", "true");
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Space_Shooter_Website.Client._Imports).Assembly);

app.Run();
