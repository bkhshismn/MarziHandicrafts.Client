using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Extensions;
using Marzi.Client.Infrastructure.Storage;
using Marzi.Client.Shared.Layouts;
using Marzi.Client.Web.Authentication;
using Marzi.Client.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
