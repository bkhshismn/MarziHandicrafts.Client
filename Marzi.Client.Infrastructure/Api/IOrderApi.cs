using Marzi.Client.Core.Models.Orders;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IOrderApi
{
    // ============================================================
    // Admin - Get All Orders
    // ============================================================

    [Get("/api/orders/admin")]
    Task<ApiResponse<List<OrderDto>>> GetAllForAdminAsync();


    // ============================================================
    // User - Get My Orders
    // ============================================================

    [Get("/api/orders/user/{userId}")]
    Task<ApiResponse<List<OrderDto>>> GetByUserAsync(
        Guid userId);


    // ============================================================
    // Get Order By Id
    // ============================================================

    [Get("/api/orders/{id}")]
    Task<ApiResponse<OrderDto>> GetByIdAsync(
        Guid id);


    // ============================================================
    // Create Order
    // ============================================================

    [Post("/api/orders")]
    Task<ApiResponse<OrderDto>> CreateAsync(
        [Body] CreateOrderRequest request);


    // ============================================================
    // Admin - Update Order
    // ============================================================

    [Put("/api/orders/{id}/admin")]
    Task<ApiResponse<OrderDto>> UpdateByAdminAsync(
        Guid id,
        [Body] AdminUpdateOrderRequest request);
}