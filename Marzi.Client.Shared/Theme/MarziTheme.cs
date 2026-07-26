using MudBlazor;

namespace Marzi.Client.Shared.Theme;

public static class MarziTheme
{
    public static MudTheme Default = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#8B5E3C",
            Secondary = "#D2B48C",
            Background = "#FAF8F4",
            Surface = "#FFFFFF",
            AppbarBackground = "#8B5E3C",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#FFFFFF",
            DrawerText = "#5A3E2B",
            TextPrimary = "#3B2A1F",
            TextSecondary = "#7A6657"
        }
    };
}
