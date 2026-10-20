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

            var refreshHandler = new RefreshTokenHandler(
                sp.GetRequiredService<AuthService>(),
                tokenStorage,
                sp.GetRequiredService<TokenRefreshCoordinator>())
            {
                InnerHandler = authHandler
            };

            var httpClient =
                new HttpClient(refreshHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<ICourseApi>(httpClient);
        });

        services.AddScoped<IUsersApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage) { InnerHandler = new HttpClientHandler() };
            var refreshHandler = new RefreshTokenHandler(sp.GetRequiredService<AuthService>(), tokenStorage, sp.GetRequiredService<TokenRefreshCoordinator>()) { InnerHandler = authHandler };
            return RestService.For<IUsersApi>(new HttpClient(refreshHandler) { BaseAddress = new Uri(baseUrl) });
        });

        services.AddScoped<ICourseCategoriesApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage) { InnerHandler = new HttpClientHandler() };
            var refreshHandler = new RefreshTokenHandler(sp.GetRequiredService<AuthService>(), tokenStorage, sp.GetRequiredService<TokenRefreshCoordinator>()) { InnerHandler = authHandler };
            return RestService.For<ICourseCategoriesApi>(new HttpClient(refreshHandler) { BaseAddress = new Uri(baseUrl) });
        });

        services.AddScoped<IPortfolioApi>(sp =>
        {
            var tokenStorage =
                sp.GetRequiredService<ITokenStorage>();

            var authHandler =
                new AuthorizationHandler(tokenStorage)
                {
                    InnerHandler = new HttpClientHandler()
                };

            var refreshHandler =
                new RefreshTokenHandler(
                    sp.GetRequiredService<AuthService>(),
                    tokenStorage,
                    sp.GetRequiredService<TokenRefreshCoordinator>())
                {
                    InnerHandler = authHandler
                };

            var httpClient =
                new HttpClient(refreshHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<IPortfolioApi>(httpClient);
        });

        // ==========================
        // Protected APIs — Scoped Refit Client
        // ==========================

        services.AddScoped<IOrderApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage)
            {
                InnerHandler = new HttpClientHandler()
            };

            var refreshHandler = new RefreshTokenHandler(
                sp.GetRequiredService<AuthService>(), tokenStorage,
                sp.GetRequiredService<TokenRefreshCoordinator>())
            {
                InnerHandler = authHandler
            };

            var httpClient =
                new HttpClient(refreshHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<IOrderApi>(httpClient);
        });


        services.AddScoped<ICourseEpisodeApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage)
            {
                InnerHandler = new HttpClientHandler()
            };

            var refreshHandler = new RefreshTokenHandler(
                sp.GetRequiredService<AuthService>(), tokenStorage,
                sp.GetRequiredService<TokenRefreshCoordinator>())
            {
                InnerHandler = authHandler
            };

            var httpClient =
                new HttpClient(refreshHandler)
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromMinutes(30)
                };

            return RestService.For<ICourseEpisodeApi>(httpClient);
        });


        services.AddScoped<ICourseLessonProgressApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage)
            {
                InnerHandler = new HttpClientHandler()
            };

            var refreshHandler = new RefreshTokenHandler(
                sp.GetRequiredService<AuthService>(), tokenStorage,
                sp.GetRequiredService<TokenRefreshCoordinator>())
            {
                InnerHandler = authHandler
            };

            var httpClient =
                new HttpClient(refreshHandler)
                {
                    BaseAddress = new Uri(baseUrl)
                };

            return RestService.For<ICourseLessonProgressApi>(
                httpClient);
        });

        services.AddScoped<IMediaApi>(sp =>
        {
            var tokenStorage = sp.GetRequiredService<ITokenStorage>();
            var authHandler = new AuthorizationHandler(tokenStorage)
            {
                InnerHandler = new HttpClientHandler()
            };

            var refreshHandler = new RefreshTokenHandler(
                sp.GetRequiredService<AuthService>(),
                tokenStorage,
                sp.GetRequiredService<TokenRefreshCoordinator>())
            {
                InnerHandler = authHandler
            };

            var httpClient = new HttpClient(refreshHandler)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromMinutes(5)
            };

            return RestService.For<IMediaApi>(httpClient);
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
        services.AddScoped<MediaService>();

        return services;
    }
}
