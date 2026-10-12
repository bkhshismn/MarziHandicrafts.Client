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

    public static MudTheme Default => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = ThemeColors.Primary,
            Secondary = ThemeColors.Secondary,
            Background = ThemeColors.Background,
            Surface = ThemeColors.Surface,

            AppbarBackground = ThemeColors.Primary,
            AppbarText = "#FFFFFF",

            DrawerBackground = ThemeColors.Surface,
            DrawerText = "#5A3E2B",

            TextPrimary = ThemeColors.TextPrimary,
            TextSecondary = ThemeColors.TextSecondary,

            Success = ThemeColors.Success,
            Error = ThemeColors.Error
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
