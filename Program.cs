using Microsoft.EntityFrameworkCore;
using WeatherAPI.DataContext;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<WeatherService>();

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


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
Task.Run(async () =>
{
    while (true)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var weatherService = scope.ServiceProvider.GetRequiredService<WeatherService>();
            await weatherService.FetchWeatherDataAsync();
        }
        await Task.Delay(TimeSpan.FromMinutes(1));
    }
});

app.Run();
