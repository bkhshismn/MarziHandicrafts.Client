using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Extensions;
using Marzi.Client.Infrastructure.Storage;
using Marzi.Client.Shared.Layouts;
using Marzi.Client.Shared.Services;
using Marzi.Client.Shared.Theme;
using Marzi.Client.Web.Authentication;
using Marzi.Client.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Avoid the Windows Event Log provider: on developer machines without event-log
// write access, Data Protection key creation can fail while reporting warnings.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

if (builder.Environment.IsDevelopment())
{
    var keyDirectory = Path.Combine(
        Path.GetTempPath(),
        "MarziHandicrafts",
        "DataProtection-Keys");

    Directory.CreateDirectory(keyDirectory);
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(keyDirectory));
}

var apiBaseUrl =
    builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException(
        "Api:BaseUrl is not configured.");

builder.Services.AddScoped<ILocalStorage, LocalStorage>();
builder.Services.AddScoped<ShopCartService>();
builder.Services.AddScoped<ThemePreferenceService>();

builder.Services.AddInfrastructure(apiBaseUrl);

// ========================================
// Authentication / Authorization
// ========================================

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<IAuthenticationNotifier>(
    sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

// مهم: CascadingAuthenticationState را به DI اضافه می‌کند
builder.Services.AddCascadingAuthenticationState();

// ========================================
// Build
// ========================================

var app = builder.Build();

var versionMaterial = new StringBuilder();
foreach (var assemblyPath in Directory
             .EnumerateFiles(AppContext.BaseDirectory, "*.dll")
             .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
{
    using var assemblyStream = File.OpenRead(assemblyPath);
    versionMaterial.Append(Path.GetFileName(assemblyPath));
    versionMaterial.Append(':');
    versionMaterial.Append(Convert.ToHexString(SHA256.HashData(assemblyStream)));
    versionMaterial.Append('|');
}

var appVersion = Convert.ToHexString(
    SHA256.HashData(Encoding.UTF8.GetBytes(versionMaterial.ToString())))[..16];

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

// فعلاً برای development خاموش
// app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapGet("/app-version", (HttpContext context) =>
{
    context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
    context.Response.Headers.Pragma = "no-cache";
    context.Response.Headers.Expires = "0";

    return Results.Json(new { version = appVersion });
});

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly);


// ========================================
// SEO: sitemap.xml
// ========================================

app.MapGet("/sitemap.xml", async (IConfiguration configuration) =>
{
    var apiBaseUrl = configuration["Api:BaseUrl"];

    var publicBaseUrl =
        configuration["Media:PublicBaseUrl"]?.TrimEnd('/')
        ?? "https://www.marzihandicrafts.ir";

    var urls = new List<string>
    {
        "/",
        "/courses",
        "/portfolio",
        "/shop",
        "/about",
        "/contact"
    };

    try
    {
        if (!string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            using var http = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            var courses =
                await http.GetFromJsonAsync<List<CourseSitemapItem>>(
                    "/api/courses/published");

            if (courses is not null)
                urls.AddRange(courses.Select(c => $"/courses/{c.Id}"));

            var items =
                await http.GetFromJsonAsync<List<PortfolioSitemapItem>>(
                    "/api/portfolio/published");

            if (items is not null)
                urls.AddRange(items.Select(p => $"/portfolio/{p.Id}"));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SITEMAP] Could not load content: {ex.Message}");
    }

    var xml = new StringBuilder();

    xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    xml.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

    foreach (var url in urls.Distinct())
    {
        xml.AppendLine("  <url>");
        xml.AppendLine($"    <loc>{publicBaseUrl}{url}</loc>");
        xml.AppendLine("  </url>");
    }

    xml.AppendLine("</urlset>");

    return Results.Content(xml.ToString(), "application/xml", Encoding.UTF8);
});


app.Run();

public sealed record CourseSitemapItem(Guid Id);

public sealed record PortfolioSitemapItem(Guid Id);
