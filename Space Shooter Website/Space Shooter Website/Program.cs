using Space_Shooter_Website.Client.Pages;
using Space_Shooter_Website.Components;
using Client_Manager___API;

var builder = WebApplication.CreateBuilder(args);

// 2. Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 3. Register your API Service for the Server (Prerendering)
//builder.Services.AddScoped<IApiService, ApiService>(); - For scoped: lifetime is per loading of the component
builder.Services.AddSingleton<IApiService, ApiService>();// - For singleton: lifetime is per application

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
