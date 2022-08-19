using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using PlanIt.Presentation.WebApp.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .Configure<PlanItBackendApiUrls>(builder.Configuration.GetSection(nameof(PlanItBackendApiUrls)))
    .AddHttpClient()
    .AddLocalization(options => options.ResourcesPath = "Resources")
    .AddControllersWithViews()
    .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Generic");
    app.UseHsts();
}

#region Localization
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("it"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Plan}/{action=Create}/");

app.Run();
