using System.Security.Claims;
using Marzi.Client.Core.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Marzi.Client.Web.Authentication;

public sealed class CustomAuthenticationStateProvider
    : AuthenticationStateProvider,
      IAuthenticationNotifier
{
    private readonly ITokenStorage _tokenStorage;
    private readonly IAuthenticationTokenStore _tokenStore;

    private static readonly ClaimsPrincipal Anonymous =
        new(new ClaimsIdentity());

    private bool _initialized;

    public CustomAuthenticationStateProvider(
        ITokenStorage tokenStorage,
        IAuthenticationTokenStore tokenStore)
    {
        _tokenStorage = tokenStorage;
        _tokenStore = tokenStore;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        try
        {
            var accessToken =
                await _tokenStorage.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(accessToken))
                return CreateAnonymousState();

            return CreateAuthenticatedState(accessToken);
        }
        catch (InvalidOperationException)
        {
            return CreateAnonymousState();
        }
        catch (JSException)
        {
            return CreateAnonymousState();
        }
        catch
        {
            return CreateAnonymousState();
        }
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        try
        {
            await _tokenStorage.RestoreAsync();

            var accessToken =
                await _tokenStorage.GetAccessTokenAsync();

            var refreshToken =
                await _tokenStorage.GetRefreshTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                if (!string.IsNullOrWhiteSpace(refreshToken))
                {
                    _tokenStore.SetTokens(
                        accessToken,
                        refreshToken);
                }
                else
                {
                    _tokenStore.SetAccessToken(
                        accessToken);
                }
            }

            _initialized = true;

            var state =
                await GetAuthenticationStateAsync();

            Console.WriteLine(
                $"[AUTH INIT] IsAuthenticated: {state.User.Identity?.IsAuthenticated}");

            Console.WriteLine(
                $"[AUTH INIT] IsAdmin: {state.User.IsInRole("Admin")}");

            NotifyAuthenticationStateChanged(
                Task.FromResult(state));
        }
        catch (InvalidOperationException)
        {
            // JS هنوز آماده نیست.
            // در تلاش بعدی دوباره Restore می‌شود.
            return;
        }
        catch (JSException)
        {
            return;
        }
    }

    public void NotifyAuthenticated(
        string accessToken)
    {
        _tokenStore.SetAccessToken(accessToken);

        var state =
            CreateAuthenticatedState(accessToken);

        NotifyAuthenticationStateChanged(
            Task.FromResult(state));
    }

    public void NotifyLoggedOut()
    {
        _initialized = true;

        _tokenStore.Clear();

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                CreateAnonymousState()));
    }

    public async Task RefreshAsync()
    {
        var state =
            await GetAuthenticationStateAsync();

        NotifyAuthenticationStateChanged(
            Task.FromResult(state));
    }

    private static AuthenticationState
        CreateAuthenticatedState(string accessToken)
    {
        var claims =
            JwtClaimsParser.ParseClaimsFromJwt(accessToken)
                .ToList();

        Console.WriteLine(
            $"[AUTH STATE] Claims count: {claims.Count}");

        foreach (var claim in claims)
        {
            Console.WriteLine(
                $"[AUTH STATE] {claim.Type} = {claim.Value}");
        }

        var identity =
            new ClaimsIdentity(
                claims,
                authenticationType: "jwt",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

        var principal =
            new ClaimsPrincipal(identity);

        Console.WriteLine(
            $"[AUTH STATE] IsAuthenticated: {principal.Identity?.IsAuthenticated}");

        Console.WriteLine(
            $"[AUTH STATE] IsAdmin: {principal.IsInRole("Admin")}");

        Console.WriteLine(
            $"[AUTH STATE] IsInstructor: {principal.IsInRole("Instructor")}");

        return new AuthenticationState(principal);
    }

    private static AuthenticationState
        CreateAnonymousState()
    {
        return new AuthenticationState(
            Anonymous);
    }
}