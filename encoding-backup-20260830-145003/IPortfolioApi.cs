using Marzi.Client.Core.Models.Portfolio;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IPortfolioApi
{
    [Get("/api/portfolio/published")]
    Task<ApiResponse<List<PortfolioItemDto>>> GetPublishedAsync();

    [Get("/api/portfolio/category/{category}")]
    Task<ApiResponse<List<PortfolioItemDto>>> GetByCategoryAsync(string category);

    [Get("/api/portfolio/{id}")]
    Task<ApiResponse<PortfolioItemDto>> GetByIdAsync(Guid id);
}