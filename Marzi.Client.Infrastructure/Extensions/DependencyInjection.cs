using Microsoft.Extensions.DependencyInjection;
using Refit;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string baseUrl)
    {
        services

            .AddRefitClient<IAuthApi>()

            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(baseUrl);
            });

        services

            .AddRefitClient<ICourseApi>()

            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(baseUrl);
            });

        services

            .AddRefitClient<IPortfolioApi>()

            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(baseUrl);
            });

        services

            .AddRefitClient<IOrderApi>()

            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(baseUrl);
            });

        return services;
    }
}