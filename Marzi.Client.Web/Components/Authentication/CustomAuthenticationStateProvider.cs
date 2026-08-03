using System.Security.Claims;
using Marzi.Client.Core.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Marzi.Client.Web.Authentication;

public sealed class CustomAuthenticationStateProvider
    : AuthenticationStateProvider,
      IAuthenticationNotifier
{
    private readonly ITokenStorage _tokenStorage;

    private static readonly ClaimsPrincipal Anonymous =
        new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(
        ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        var accessToken =
            await _tokenStorage.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(accessToken))
            return CreateAnonymousState();

        try
        {
            return CreateAuthenticatedState(accessToken);
        }
        catch
        {
            await _tokenStorage.ClearAsync();

            return CreateAnonymousState();
        }
    }

    public void NotifyAuthenticated(string accessToken)
    {
        var state =
            CreateAuthenticatedState(accessToken);

        NotifyAuthenticationStateChanged(
            Task.FromResult(state));
    }

    public void NotifyLoggedOut()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(CreateAnonymousState()));
    }

    public Task RefreshAsync()
    {
        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());

        return Task.CompletedTask;
    }

    private static AuthenticationState
        CreateAuthenticatedState(string accessToken)
    {
        var claims =
            JwtClaimsParser.ParseClaimsFromJwt(accessToken);

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "jwt");

        return new AuthenticationState(
            new ClaimsPrincipal(identity));
    }

    private static AuthenticationState
        CreateAnonymousState()
    {
        return new AuthenticationState(Anonymous);
    }
}