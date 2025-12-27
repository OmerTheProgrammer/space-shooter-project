using Client_Manager___API;

var builder = WebApplication.CreateBuilder(args);

// 1. Get the URL from the appsettings.Development.json
string serverUrl = builder.Configuration["ConnectionStrings:SpaceShooterServer"];
// 2. Inject it into your ApiService
// This uses the constructor you wrote: public ApiService(string baseUri)
builder.Services.AddScoped<IApiService>(sp => new ApiService(serverUrl));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



