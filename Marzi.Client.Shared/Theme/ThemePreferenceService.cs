using System.Security.Claims;
using Marzi.Client.Core.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Marzi.Client.Shared.Theme;

public sealed class ThemePreferenceService(
    ILocalStorage localStorage,
    AuthenticationStateProvider authenticationStateProvider,
    IJSRuntime jsRuntime)
{
    private const string AnonymousStorageKey = "marzi.theme.anonymous";
    private string _storageKey = AnonymousStorageKey;

    public string SelectedThemeId { get; private set; } = "terracotta";
    public MudTheme CurrentTheme => AppTheme.For(SelectedThemeId);
    public event Action? Changed;

    public async Task InitializeAsync()
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("sub")?.Value
                     ?? user.FindFirst("nameid")?.Value;
        _storageKey = string.IsNullOrWhiteSpace(userId)
            ? AnonymousStorageKey
            : $"marzi.theme.user.{userId}";

        var saved = await localStorage.GetAsync<string>(_storageKey);
        var selected = AppTheme.GetOption(saved).Id;
        SelectedThemeId = selected;
        await ApplyCssVariablesAsync();
        Changed?.Invoke();
    }

    public async Task SelectAsync(string themeId)
    {
        SelectedThemeId = AppTheme.GetOption(themeId).Id;
        await localStorage.SetAsync(_storageKey, SelectedThemeId);
        await ApplyCssVariablesAsync();
        Changed?.Invoke();
    }

    private async Task ApplyCssVariablesAsync()
    {
        var theme = AppTheme.GetOption(SelectedThemeId);
        await jsRuntime.InvokeVoidAsync("marziTheme.apply", theme.Background, theme.TextPrimary, theme.Primary, theme.TextSecondary);
    }
}
