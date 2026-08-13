using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Extensions;
using Marzi.Client.Infrastructure.Storage;
using Marzi.Client.Shared.Layouts;
using Marzi.Client.Web.Authentication;
using Marzi.Client.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

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

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly);

app.Run();