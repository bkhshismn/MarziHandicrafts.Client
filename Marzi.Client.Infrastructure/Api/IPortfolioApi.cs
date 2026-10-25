using Marzi.Client.Core.Models.Portfolio;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IPortfolioApi
{
    [Get("/api/portfolio")]
    Task<ApiResponse<List<PortfolioItemDto>>> GetAllAsync();

    [Get("/api/portfolio/published")]
    Task<ApiResponse<List<PortfolioItemDto>>> GetPublishedAsync();

    [Post("/api/portfolio")]
    Task<ApiResponse<PortfolioItemDto>> CreateAsync(
        [Body] PortfolioCreateRequest request);

    [Put("/api/portfolio/{id}")]
    Task<ApiResponse<PortfolioItemDto>> UpdateAsync(Guid id, [Body] PortfolioCreateRequest request);

    [Delete("/api/portfolio/{id}")]
    Task<IApiResponse> DeleteAsync(Guid id);

    [Get("/api/portfolio/category/{category}")]
    Task<ApiResponse<List<PortfolioItemDto>>> GetByCategoryAsync(string category);

    [Get("/api/portfolio/{id}")]
    Task<ApiResponse<PortfolioItemDto>> GetByIdAsync(Guid id);

    [Post("/api/portfolio/{portfolioId}/images")]
    Task<ApiResponse<PortfolioImageDto>> AddImageAsync(
        Guid portfolioId,
        [Body] PortfolioImageCreateRequest request);
}

public sealed record PortfolioCreateRequest(
    string Title,
    string Category,
    string Description,
    bool IsVisible);

public sealed record PortfolioImageCreateRequest(
    string ImageUrl);
