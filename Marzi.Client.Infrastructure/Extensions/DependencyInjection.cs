using Marzi.Client.Core.Authentication;
using Marzi.Client.Infrastructure.Api;
using Marzi.Client.Infrastructure.Authentication;
using Marzi.Client.Infrastructure.Http;
using Marzi.Client.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Marzi.Client.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException(
                "API BaseUrl is required.",
                nameof(baseUrl));
        }

        // Token storage
        services.AddScoped<ITokenStorage, TokenStorage>();

        // HTTP handlers
        services.AddTransient<AuthorizationHandler>();
        services.AddTransient<RefreshTokenHandler>();

        // Auth API
        // عمداً هیچ Authorization/Refresh handler ندارد.
        services
            .AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

        // Courses API
        services
            .AddRefitClient<ICourseApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

        // Portfolio API
        services
            .AddRefitClient<IPortfolioApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

        // Orders API
        services
            .AddRefitClient<IOrderApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

        // Application services
        services.AddScoped<AuthService>();
        services.AddScoped<CourseService>();
        services.AddScoped<TokenRefreshCoordinator>();

        return services;
    }
}