using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Extensions;
using Marzi.Client.Infrastructure.Services;
using Marzi.Client.Infrastructure.Storage;
using Marzi.Client.Web.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddInfrastructure(builder.Configuration["Api:BaseUrl"]!);
builder.Services.AddScoped<ILocalStorage, LocalStorage>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// ✅ فقط یکی از این دو خط را نگه دارید:

// گزینه ۱: استفاده از App.razor در پروژه Web (پیشنهادی)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// گزینه ۲: استفاده از App.razor در پروژه Shared
// app.MapRazorComponents<Marzi.Client.Shared.App>()
//     .AddInteractiveServerRenderMode();

app.Run();