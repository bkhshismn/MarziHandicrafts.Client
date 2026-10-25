namespace Marzi.Client.Shared.Services;

public sealed record ShopProduct(
    string Id,
    string Name,
    string Category,
    string Description,
    string ImagePosition,
    string Badge,
    string Availability)
{
    public string ImageUrl { get; init; } = "/images/hero-handicrafts.png";
}

public static class ShopCatalog
{
    private static IReadOnlyList<ShopProduct> _products =
    [
        new("portrait-bust", "سردیس پرتره دست‌ساز", "سردیس و چهره",
            "چهره‌پردازی با جزئیات و پرداخت دستی؛ هر اثر با ویژگی‌های منحصربه‌فرد خودش ساخته می‌شود.", "18% center", "ساخت سفارشی", "زمان و هزینه پس از هماهنگی اعلام می‌شود"),
        new("decorative-mask", "ماسک دکوراتیو", "ماسک و دیوارکوب",
            "اثری دکوراتیو با بافت و رنگ‌آمیزی دستی، مناسب برای گوشه‌ای متفاوت در خانه.", "50% center", "تک‌نسخه", "موجودی و قیمت نیازمند تأیید کارگاه است"),
        new("paper-mache-sculpture", "مجسمه پاپیه‌ماشه", "مجسمه دکوراتیو",
            "فرم‌گرفته و پرداخت‌شده با دست؛ سبک، ظریف و مناسب چیدمان فضاهای هنری.", "34% 63%", "هنر دست", "موجودی و قیمت نیازمند تأیید کارگاه است"),
        new("miniature-portrait", "پرتره هنری", "سردیس و چهره",
            "پرتره‌ای برای هدیه یا یادگاری؛ جزئیات ساخت پس از گفت‌وگو با هنرمند مشخص می‌شود.", "15% 47%", "سفارشی‌سازی", "قیمت پس از بررسی جزئیات اعلام می‌شود"),
        new("table-sculpture", "تندیس رومیزی", "مجسمه دکوراتیو",
            "یک قطعه دکوراتیو با شخصیت مستقل که با توجه به سبک و رنگ دلخواه قابل هماهنگی است.", "42% 71%", "تک‌نسخه", "موجودی و قیمت نیازمند تأیید کارگاه است"),
        new("art-mask", "ماسک هنری رنگی", "ماسک و دیوارکوب",
            "ترکیب بافت، رنگ و فرم برای دیوار یا فضای کار؛ هر نمونه با دست پرداخت می‌شود.", "30% 25%", "ساخت سفارشی", "زمان و هزینه پس از هماهنگی اعلام می‌شود")
    ];

    public static IReadOnlyList<ShopProduct> Products => _products;
    public static void SetProducts(IEnumerable<ShopProduct> products) => _products = products.ToList();
    public static ShopProduct? Find(string id) => Products.FirstOrDefault(x => x.Id == id);
}

public sealed record ShopCartLine(string ProductId, int Quantity);

public sealed class ShopCheckoutDetails
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
