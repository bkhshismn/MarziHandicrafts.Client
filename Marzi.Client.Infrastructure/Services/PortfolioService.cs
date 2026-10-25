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

    public async Task<IReadOnlyList<PortfolioItemDto>> GetAllAsync()
    {
        var response = await _api.GetAllAsync();

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت نمونه‌کارها با خطا مواجه شد.");
        }

        return response.Content;
    }

    public async Task<PortfolioItemDto> CreateAsync(
        PortfolioCreateRequest request)
    {
        var response = await _api.CreateAsync(request);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "ایجاد نمونه‌کار با خطا مواجه شد.");
        }

        return response.Content;
    }

    public async Task UpdateAsync(Guid id, PortfolioCreateRequest request)
    {
        var response = await _api.UpdateAsync(id, request);
        if (!response.IsSuccessStatusCode || response.Content is null)
            throw new InvalidOperationException("ویرایش نمونه‌کار با خطا مواجه شد.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _api.DeleteAsync(id);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("حذف نمونه‌کار با خطا مواجه شد.");
    }

    public async Task AddImageAsync(
        Guid portfolioId,
        string imageUrl)
    {
        var response = await _api.AddImageAsync(
            portfolioId,
            new PortfolioImageCreateRequest(imageUrl));

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "افزودن تصویر نمونه‌کار با خطا مواجه شد.");
        }
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
