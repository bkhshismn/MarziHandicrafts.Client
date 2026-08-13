using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;

namespace Marzi.Client.Infrastructure.Authentication;

public sealed class TokenStorage : ITokenStorage
{
    private readonly ILocalStorage _storage;

    private string? _accessToken;
    private string? _refreshToken;
    private bool _restored;

    public TokenStorage(ILocalStorage storage)
    {
        _storage = storage;
    }

    public async Task RestoreAsync()
    {
        if (_restored)
            return;

        try
        {
            _accessToken = await _storage.GetAsync<string>(AuthStorageKeys.AccessToken);
            _refreshToken = await _storage.GetAsync<string>(AuthStorageKeys.RefreshToken);
            _restored = true;

            Console.WriteLine(
                $"[TOKEN RESTORE] AccessToken exists: {!string.IsNullOrWhiteSpace(_accessToken)}");
            Console.WriteLine(
                $"[TOKEN RESTORE] RefreshToken exists: {!string.IsNullOrWhiteSpace(_refreshToken)}");
        }
        catch (InvalidOperationException)
        {
        }
        catch (Microsoft.JSInterop.JSException)
        {
        }
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_accessToken))
        {
            try
            {
                _accessToken = await _storage.GetAsync<string>(AuthStorageKeys.AccessToken);
            }
            catch
            {
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
                _refreshToken = await _storage.GetAsync<string>(AuthStorageKeys.RefreshToken);
            }
            catch
            {
            }
        }

        return _refreshToken;
    }

    public async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
        _restored = true;

        await _storage.SetAsync(AuthStorageKeys.AccessToken, accessToken);
        await _storage.SetAsync(AuthStorageKeys.RefreshToken, refreshToken);

        Console.WriteLine("[TOKEN STORAGE] Tokens saved.");
    }

    public async Task ClearAsync()
    {
        _accessToken = null;
        _refreshToken = null;
        _restored = true;

        await _storage.RemoveAsync(AuthStorageKeys.AccessToken);
        await _storage.RemoveAsync(AuthStorageKeys.RefreshToken);

        Console.WriteLine("[TOKEN STORAGE] Tokens cleared.");
    }
}