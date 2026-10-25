using Marzi.Client.Core.Interfaces;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Shared.Services;

public sealed class ShopCartService(ILocalStorage storage, IShopProductsApi productsApi)
{
    private const string CartKey = "marzi.shop.cart.v1";
    private const string CheckoutKey = "marzi.shop.checkout.v1";
    private List<ShopCartLine> _items = [];
    private bool _loaded;

    public event Action? Changed;
    public IReadOnlyList<ShopCartLine> Items => _items;
    public int ItemCount => _items.Sum(item => item.Quantity);
    public bool IsEmpty => _items.Count == 0;

    public async Task LoadAsync()
    {
        if (_loaded) return;
        var loaded = false;
        try
        {
            var products = await productsApi.ListAsync();
            if (products.IsSuccessStatusCode && products.Content is not null)
                ShopCatalog.SetProducts(products.Content.Select(x => new ShopProduct(x.Id, x.Name, x.Category, x.Description, x.ImagePosition, x.Badge, x.Availability) { ImageUrl = x.ImageUrl }));
            _items = await storage.GetAsync<List<ShopCartLine>>(CartKey) ?? [];
            _items = _items.Where(x => ShopCatalog.Find(x.ProductId) is not null && x.Quantity > 0)
                .GroupBy(x => x.ProductId)
                .Select(group => new ShopCartLine(group.Key, group.Sum(x => x.Quantity)))
                .ToList();
            loaded = true;
        }
        catch
        {
            _items = [];
        }
        // Retry if prerendering prevented JavaScript localStorage access.
        _loaded = loaded;
        Changed?.Invoke();
    }

    public async Task AddAsync(string productId)
    {
        if (ShopCatalog.Find(productId) is null) return;
        var index = _items.FindIndex(x => x.ProductId == productId);
        if (index < 0) _items.Add(new ShopCartLine(productId, 1));
        else _items[index] = _items[index] with { Quantity = _items[index].Quantity + 1 };
        await SaveAsync();
    }

    public async Task SetQuantityAsync(string productId, int quantity)
    {
        var index = _items.FindIndex(x => x.ProductId == productId);
        if (index < 0) return;
        if (quantity < 1) _items.RemoveAt(index);
        else _items[index] = _items[index] with { Quantity = Math.Min(quantity, 20) };
        await SaveAsync();
    }

    public async Task RemoveAsync(string productId)
    {
        _items.RemoveAll(x => x.ProductId == productId);
        await SaveAsync();
    }

    public async Task ClearAsync()
    {
        _items.Clear();
        await SaveAsync();
    }

    public Task SaveCheckoutAsync(ShopCheckoutDetails details) => storage.SetAsync(CheckoutKey, details);

    public async Task<ShopCheckoutDetails> GetCheckoutAsync() =>
        await storage.GetAsync<ShopCheckoutDetails>(CheckoutKey) ?? new ShopCheckoutDetails();

    private async Task SaveAsync()
    {
        await storage.SetAsync(CartKey, _items);
        Changed?.Invoke();
    }
}
