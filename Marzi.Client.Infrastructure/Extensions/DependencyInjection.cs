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

        // Authentication
        services.AddScoped<ITokenStorage, TokenStorage>();

        // Http Handlers
        services.AddTransient<AuthorizationHandler>();
        services.AddTransient<RefreshTokenHandler>();

        // ==========================
        // Public APIs
        // ==========================

        services
            .AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        services
            .AddRefitClient<ICourseApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        services
            .AddRefitClient<IPortfolioApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        // ==========================
        // Protected APIs
        // ==========================

        services
            .AddRefitClient<IOrderApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

        // ==========================
        // Services
        // ==========================

        services.AddScoped<AuthService>();
        services.AddScoped<CourseService>();
        services.AddScoped<PortfolioService>();

        //services.AddScoped<PortfolioService>();
        //services.AddScoped<OrderService>();

        services.AddScoped<TokenRefreshCoordinator>();

        return services;
    }
}