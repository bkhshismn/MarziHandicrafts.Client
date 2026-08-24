namespace Marzi.Client.Core.Models.Orders;

public sealed class AdminUpdateOrderRequest
{
    public decimal? EstimatedPrice { get; set; }

    public string Status { get; set; } = "Pending";
}