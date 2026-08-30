using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Extensions;
using Marzi.Client.Infrastructure.Services;
using Marzi.Client.Maui.Storage; 
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace Marzi.Client.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddInfrastructure( "https://localhost:5001");
        builder.Services.AddScoped<ILocalStorage, PreferencesStorage>();
        builder.Services.AddScoped<AuthService>();
#endif

        return builder.Build();
	}
}
