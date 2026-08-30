using Marzi.Client.Core.Models.Portfolio;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Infrastructure.Services;

public sealed class PortfolioService
{
    private readonly IPortfolioApi _api;

    public PortfolioService(IPortfolioApi api)
    {
        _api = api;
    }

    public async Task<IReadOnlyList<PortfolioItemDto>> GetPublishedAsync()
    {
        var response = await _api.GetPublishedAsync();

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت نمونه‌کارها با خطا مواجه شد.");
        }

        return response.Content;
    }

    public async Task<IReadOnlyList<PortfolioItemDto>> GetByCategoryAsync(
        string category)
    {
        var response =
            await _api.GetByCategoryAsync(category);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت نمونه‌کارها با خطا مواجه شد.");
        }

        return response.Content;
    }
    public async Task<PortfolioItemDto> GetByIdAsync(Guid id)
    {
        var response = await _api.GetByIdAsync(id);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "نمونه‌کار پیدا نشد.");
        }

        return response.Content;
    }
}