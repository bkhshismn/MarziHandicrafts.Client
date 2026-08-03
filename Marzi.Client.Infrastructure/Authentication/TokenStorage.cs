using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Interfaces;

namespace Marzi.Client.Infrastructure.Authentication;

public sealed class TokenStorage : ITokenStorage
{
    private readonly ILocalStorage _storage;

    public TokenStorage(ILocalStorage storage)
    {
        _storage = storage;
    }

    public Task<string?> GetAccessTokenAsync()
        => _storage.GetAsync<string>(AuthStorageKeys.AccessToken);

    public Task<string?> GetRefreshTokenAsync()
        => _storage.GetAsync<string>(AuthStorageKeys.RefreshToken);

    public async Task SaveTokensAsync(
        string accessToken,
        string refreshToken)
    {
        await _storage.SetAsync(
            AuthStorageKeys.AccessToken,
            accessToken);

        await _storage.SetAsync(
            AuthStorageKeys.RefreshToken,
            refreshToken);
    }

    public async Task ClearAsync()
    {
        await _storage.RemoveAsync(AuthStorageKeys.AccessToken);
        await _storage.RemoveAsync(AuthStorageKeys.RefreshToken);
    }
}