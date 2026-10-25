using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IShopProductsApi
{
    [Get("/api/shop-products")]
    Task<ApiResponse<List<ShopProductDto>>> ListAsync();
    [Get("/api/shop-products/manage")]
    Task<ApiResponse<List<ShopProductDto>>> ManageAsync();
    [Post("/api/shop-products")]
    Task<ApiResponse<ShopProductDto>> CreateAsync([Body] ShopProductRequest request);
    [Put("/api/shop-products/{id}")]
    Task<ApiResponse<ShopProductDto>> UpdateAsync(string id, [Body] ShopProductRequest request);
    [Delete("/api/shop-products/{id}")]
    Task<IApiResponse> DeleteAsync(string id);
}

public sealed class ShopProductDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string ImagePosition { get; set; } = "center";
    public string Badge { get; set; } = "";
    public string Availability { get; set; } = "";
    public bool IsPublished { get; set; }
}

public sealed class ShopProductRequest
{
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string ImagePosition { get; set; } = "center";
    public string Badge { get; set; } = "دست‌ساز";
    public string Availability { get; set; } = "هماهنگی با کارگاه";
    public bool IsPublished { get; set; } = true;
}
