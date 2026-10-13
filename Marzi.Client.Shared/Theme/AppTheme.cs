using MudBlazor;

namespace Marzi.Client.Shared.Theme;

public static class AppTheme
{
    private static readonly string[] FontFamily =
      [
          "Vazir",
        "Tahoma",
        "Arial",
        "sans-serif"
      ];

    public static MudTheme Default => For("terracotta");

    public static IReadOnlyList<ThemeOption> Options { get; } =
    [
        new("terracotta", "سفالی و زیتونی", "#925640", "#737B52", "#F7F3EC", "#FFFDFA", "#382C25", "#75675C", "#65784D", "#B85F52"),
        new("mocha", "سبز موکا (تم قبلی)", "#6B7F5A", "#C8B89A", "#F7F5F0", "#FFFFFF", "#3A342C", "#7A7165", "#5A7A4F", "#B85C5C"),
        new("ocean", "آبی اقیانوسی", "#315B70", "#4F8A86", "#EFF5F5", "#FFFFFF", "#263B45", "#60747A", "#4B8068", "#B95757"),
        new("rose", "رز و یاسی", "#87536C", "#A17B91", "#F8F1F5", "#FFFCFE", "#3D2D37", "#786875", "#648064", "#B6535F"),
        new("midnight", "شبِ لاجوردی", "#34466F", "#7183AD", "#F1F3F9", "#FFFFFF", "#293149", "#68718A", "#4F806A", "#B5535C")
    ];

    public static ThemeOption GetOption(string? id) =>
        Options.FirstOrDefault(option => option.Id == id) ?? Options[0];

    public static MudTheme For(string? id)
    {
        var colors = GetOption(id);
        return new MudTheme
        {
        PaletteLight = new PaletteLight
        {
            Primary = colors.Primary,
            Secondary = colors.Secondary,
            Background = colors.Background,
            Surface = colors.Surface,

            AppbarBackground = colors.Primary,
            AppbarText = "#FFFFFF",

            DrawerBackground = colors.Surface,
            DrawerText = colors.TextPrimary,

            TextPrimary = colors.TextPrimary,
            TextSecondary = colors.TextSecondary,

            Success = colors.Success,
            Error = colors.Error
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = FontFamily,
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.8"
            },

            H1 = new H1Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700",
                FontSize = "2.5rem",
                LineHeight = "1.4"
            },

            H2 = new H2Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700",
                FontSize = "2rem",
                LineHeight = "1.45"
            },

            H3 = new H3Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700",
                FontSize = "1.75rem",
                LineHeight = "1.5"
            },

            H4 = new H4Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600",
                FontSize = "1.5rem",
                LineHeight = "1.55"
            },

            H5 = new H5Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600",
                FontSize = "1.25rem",
                LineHeight = "1.6"
            },

            H6 = new H6Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600",
                FontSize = "1.1rem",
                LineHeight = "1.7"
            },

            Body1 = new Body1Typography
            {
                FontFamily = FontFamily,
                FontWeight = "400",
                FontSize = "1rem",
                LineHeight = "1.9"
            },

            Body2 = new Body2Typography
            {
                FontFamily = FontFamily,
                FontWeight = "400",
                FontSize = "0.925rem",
                LineHeight = "1.8"
            },

            Button = new ButtonTypography
            {
                FontFamily = FontFamily,
                FontWeight = "600",
                FontSize = "0.95rem"
            },

            Caption = new CaptionTypography
            {
                FontFamily = FontFamily,
                FontWeight = "400",
                FontSize = "0.8rem",
                LineHeight = "1.7"
            }
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "12px"
        }
        };
    }
}

public sealed record ThemeOption(
    string Id,
    string Name,
    string Primary,
    string Secondary,
    string Background,
    string Surface,
    string TextPrimary,
    string TextSecondary,
    string Success,
    string Error);
