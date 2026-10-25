using Marzi.Client.Core.Models.Orders;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Infrastructure.Services;

public sealed class OrderService
{
    private readonly IOrderApi _api;

    public OrderService(IOrderApi api)
    {
        _api = api;
    }


    // ============================================================
    // Admin - Get All Orders
    // ============================================================

    public async Task<IReadOnlyList<OrderDto>>
        GetAllForAdminAsync()
    {
        var response =
            await _api.GetAllForAdminAsync();

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت سفارش‌های ادمین با خطا مواجه شد.");
        }

        return response.Content;
    }


    // ============================================================
    // User - Get My Orders
    // ============================================================

    public async Task<IReadOnlyList<OrderDto>>
        GetByUserAsync(Guid userId)
    {
        var response =
            await _api.GetByUserAsync(userId);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت سفارش‌ها با خطا مواجه شد.");
        }

        return response.Content;
    }


    // ============================================================
    // Get Order By Id
    // ============================================================

    public async Task<OrderDto>
        GetByIdAsync(Guid id)
    {
        var response =
            await _api.GetByIdAsync(id);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "سفارش مورد نظر یافت نشد.");
        }

        return response.Content;
    }


    // ============================================================
    // Create Order
    // ============================================================

    public async Task<OrderDto>
        CreateAsync(CreateOrderRequest request)
    {
        var response =
            await _api.CreateAsync(request);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "ثبت سفارش با خطا مواجه شد.");
        }

        return response.Content;
    }

    public async Task<OrderDto> UpdateByUserAsync(Guid id, CreateOrderRequest request)
    {
        var response = await _api.UpdateByUserAsync(id, request);
        if (!response.IsSuccessStatusCode || response.Content is null)
            throw new InvalidOperationException("ذخیره تغییرات سفارش با خطا مواجه شد.");
        return response.Content;
    }

    public async Task DeleteByUserAsync(Guid id)
    {
        var response = await _api.DeleteByUserAsync(id);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException("حذف سفارش انجام نشد.");
    }

    public async Task DeleteByAdminAsync(Guid id)
    {
        var response = await _api.DeleteByAdminAsync(id);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException("حذف سفارش انجام نشد.");
    }

    public async Task<OrderDto> UpdateByAdminAsync(
    Guid id,
    AdminUpdateOrderRequest request)
    {
        var response =
            await _api.UpdateByAdminAsync(
                id,
                request);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "ذخیره تغییرات سفارش با خطا مواجه شد.");
        }

        return response.Content;
    }
}
