using MudBlazor;

namespace Marzi.Client.Shared.Theme;

public static class AppTheme
{
    public static MudTheme Default => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = ThemeColors.Primary,
            Secondary = ThemeColors.Secondary,
            Background = ThemeColors.Background,
            Surface = ThemeColors.Surface,
            Success = ThemeColors.Success,
            Error = ThemeColors.Error
        }
    };
}