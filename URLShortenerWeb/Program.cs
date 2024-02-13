using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using URLShortenerWeb.Data;
using URLShortenerWeb.Services;



var builder = WebApplication.CreateBuilder(args);

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog(); 

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SQLDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLDbConnection")));

builder.Services.AddScoped<IURLRepository, URLRepository>();
builder.Services.AddScoped<IShortURLCodeService, ShortURLCodeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=URL}/{action=GenerateURL}/{id?}");

app.MapControllerRoute(
    name: "shortUrl",
    pattern: "{shortCode}",
    defaults: new { controller = "URL", action = "RedirectTo" });

app.Run();
