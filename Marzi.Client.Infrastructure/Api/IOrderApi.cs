using Marzi.Client.Core.Models.Orders;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IOrderApi
{
    [Get("/api/orders/user/{userId}")]
    Task<ApiResponse<List<OrderDto>>> GetByUserAsync(Guid userId);

    [Get("/api/orders/{id}")]
    Task<ApiResponse<OrderDto>> GetByIdAsync(Guid id);

    [Post("/api/orders")]
    Task<ApiResponse<OrderDto>> CreateAsync(
        [Body] CreateOrderRequest request);
}