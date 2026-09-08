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

        // ==========================
        // Authentication
        // ==========================
        services.AddScoped<ITokenStorage, TokenStorage>();
        services.AddScoped<IAuthenticationTokenStore, AuthenticationTokenStore>();

        // ==========================
        // Handlers
        // ==========================
        // مهم: Scoped تا همان TokenStorage مدار را بگیرد
        services.AddScoped<AuthorizationHandler>();
        services.AddScoped<RefreshTokenHandler>();

        // ==========================
        // Public APIs (بدون Auth)
        // ==========================
        services
            .AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        services.AddScoped<ICourseApi>(sp =>
        {
            var tokenStorage =
                sp.GetRequiredService<ITokenStorage>();

            var authHandler =
                new AuthorizationHandler(tokenStorage)
                {
                    InnerHandler = new HttpClientHandler()
                };

            var httpClient =
                new HttpClient(authHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<ICourseApi>(httpClient);
        });

        services
            .AddRefitClient<IPortfolioApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        // ==========================
        // Protected APIs — Scoped Refit Client
        // ==========================

        services.AddScoped<IOrderApi>(sp =>
        {
            var authHandler = new AuthorizationHandler(
                sp.GetRequiredService<ITokenStorage>())
            {
                InnerHandler = new HttpClientHandler()
            };

            var httpClient =
                new HttpClient(authHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<IOrderApi>(httpClient);
        });


        services.AddScoped<ICourseEpisodeApi>(sp =>
        {
            var authHandler = new AuthorizationHandler(
                sp.GetRequiredService<ITokenStorage>())
            {
                InnerHandler = new HttpClientHandler()
            };

            var httpClient =
                new HttpClient(authHandler)
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromMinutes(30)
                };

            return RestService.For<ICourseEpisodeApi>(httpClient);
        });


        services.AddScoped<ICourseLessonProgressApi>(sp =>
        {
            var authHandler = new AuthorizationHandler(
                sp.GetRequiredService<ITokenStorage>())
            {
                InnerHandler = new HttpClientHandler()
            };

            var httpClient =
                new HttpClient(authHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<ICourseLessonProgressApi>(
                httpClient);
        });


        // ==========================
        // Services
        // ==========================
        services.AddScoped<AuthService>();
        services.AddScoped<CourseService>();
        services.AddScoped<PortfolioService>();
        services.AddScoped<OrderService>();
        services.AddScoped<CourseEpisodeService>();
        services.AddScoped<TokenRefreshCoordinator>();
        services.AddScoped<CourseLessonProgressService>();

        return services;
    }
}