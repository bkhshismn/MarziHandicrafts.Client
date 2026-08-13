using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;

namespace Marzi.Client.Infrastructure.Authentication;

public sealed class TokenStorage : ITokenStorage
{
    private readonly ILocalStorage _storage;

    private string? _accessToken;
    private string? _refreshToken;

    public TokenStorage(ILocalStorage storage)
    {
        _storage = storage;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_accessToken))
        {
            try
            {
                _accessToken =
                    await _storage.GetAsync<string>(
                        AuthStorageKeys.AccessToken);

                if (string.IsNullOrWhiteSpace(_refreshToken))
                {
                    _refreshToken =
                        await _storage.GetAsync<string>(
                            AuthStorageKeys.RefreshToken);
                }
            }
            catch (InvalidOperationException)
            {
                // JS Interop هنوز آماده نیست.
            }
            catch (Microsoft.JSInterop.JSException)
            {
                // JS Interop هنوز آماده نیست.
            }
        }

        Console.WriteLine(
            $"[TOKEN STORAGE] GetAccessTokenAsync | Hash={GetHashCode()} | Token exists={!string.IsNullOrWhiteSpace(_accessToken)}");

        return _accessToken;
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_refreshToken))
        {
            try
            {
                _refreshToken =
                    await _storage.GetAsync<string>(
                        AuthStorageKeys.RefreshToken);
            }
            catch (InvalidOperationException)
            {
                // JS Interop هنوز آماده نیست.
            }
            catch (Microsoft.JSInterop.JSException)
            {
                // JS Interop هنوز آماده نیست.
            }
        }

        Console.WriteLine(
            $"[TOKEN STORAGE] GetRefreshTokenAsync | Hash={GetHashCode()} | Token exists={!string.IsNullOrWhiteSpace(_refreshToken)}");

        return _refreshToken;
    }

    public async Task SaveTokensAsync(
        string accessToken,
        string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;

        await _storage.SetAsync(
            AuthStorageKeys.AccessToken,
            accessToken);

        await _storage.SetAsync(
            AuthStorageKeys.RefreshToken,
            refreshToken);

        Console.WriteLine(
            $"[TOKEN STORAGE] Tokens saved | Hash={GetHashCode()}");
    }

    public async Task ClearAsync()
    {
        _accessToken = null;
        _refreshToken = null;

        await _storage.RemoveAsync(
            AuthStorageKeys.AccessToken);

        await _storage.RemoveAsync(
            AuthStorageKeys.RefreshToken);

        Console.WriteLine(
            $"[TOKEN STORAGE] Tokens cleared | Hash={GetHashCode()}");
    }
}