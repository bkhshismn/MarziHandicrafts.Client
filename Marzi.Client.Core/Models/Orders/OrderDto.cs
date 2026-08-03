namespace Marzi.Client.Core.Models.Orders;

public sealed class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? EstimatedPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class CreateOrderRequest
{
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // بهتر است بعداً Backend خودش مقدار اولیه را تعیین کند.
    public string Status { get; set; } = "New";

    public decimal? EstimatedPrice { get; set; }
}