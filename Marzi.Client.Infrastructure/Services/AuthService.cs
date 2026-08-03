using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Requests;
using Marzi.Client.Core.Responses;
using Marzi.Client.Infrastructure.Api;
using Refit;

namespace Marzi.Client.Infrastructure.Services;

public sealed class AuthService
{
    private readonly IAuthApi _api;
    private readonly ITokenStorage _tokenStorage;
    private readonly IAuthenticationNotifier _authenticationNotifier;

    public AuthService(
        IAuthApi api,
        ITokenStorage tokenStorage,
        IAuthenticationNotifier authenticationNotifier)
    {
        _api = api;
        _tokenStorage = tokenStorage;
        _authenticationNotifier = authenticationNotifier;
    }

    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _api.LoginAsync(request);

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                return null;
            }

            await SaveAuthResponseAsync(response.Content);

            _authenticationNotifier.NotifyAuthenticated(
                response.Content.Token);

            return response.Content;
        }
        catch (ApiException)
        {
            return null;
        }
    }

    public async Task<AuthResponse?> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _api.RegisterAsync(request);

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                return null;
            }

            await SaveAuthResponseAsync(response.Content);

            _authenticationNotifier.NotifyAuthenticated(
                response.Content.Token);

            return response.Content;
        }
        catch (ApiException)
        {
            return null;
        }
    }

    public Task<string?> GetAccessTokenAsync()
    {
        return _tokenStorage.GetAccessTokenAsync();
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token =
            await _tokenStorage.GetAccessTokenAsync();

        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<bool> RefreshAsync()
    {
        var refreshToken =
            await _tokenStorage.GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            await ClearSessionAsync();
            return false;
        }

        try
        {
            var response = await _api.RefreshAsync(
                new RefreshRequest
                {
                    Token = refreshToken
                });

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                await ClearSessionAsync();
                return false;
            }

            await SaveAuthResponseAsync(response.Content);

            _authenticationNotifier.NotifyAuthenticated(
                response.Content.Token);

            return true;
        }
        catch (ApiException)
        {
            await ClearSessionAsync();
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        var accessToken =
            await _tokenStorage.GetAccessTokenAsync();

        var refreshToken =
            await _tokenStorage.GetRefreshTokenAsync();

        try
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                await _api.LogoutAsync(
                    $"Bearer {accessToken}");
            }
            else if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _api.RevokeAsync(
                    new RefreshRequest
                    {
                        Token = refreshToken
                    });
            }
        }
        catch (ApiException)
        {
            // حتی اگر Backend در دسترس نباشد،
            // session محلی باید پاک شود.
        }
        finally
        {
            await ClearSessionAsync();
        }
    }
    private Task SaveAuthResponseAsync(
        AuthResponse response)
    {
        return _tokenStorage.SaveTokensAsync(
            response.Token,
            response.RefreshToken);
    }

    private async Task ClearSessionAsync()
    {
        await _tokenStorage.ClearAsync();

        _authenticationNotifier.NotifyLoggedOut();
    }
}