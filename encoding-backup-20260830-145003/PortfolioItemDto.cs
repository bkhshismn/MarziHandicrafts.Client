namespace Marzi.Client.Core.Models.Portfolio;

public sealed class PortfolioItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<PortfolioImageDto> Images { get; set; } = [];
}

public sealed class PortfolioImageDto
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int Order { get; set; }
}