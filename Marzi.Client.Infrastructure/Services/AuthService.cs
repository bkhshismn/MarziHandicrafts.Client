using Marzi.Client.Core.Interfaces;
using Marzi.Client.Core.Requests;
using Marzi.Client.Core.Responses;
using Marzi.Client.Infrastructure.Api;
using Refit;

namespace Marzi.Client.Infrastructure.Services;

public class AuthService
{
    private readonly IAuthApi _api;
    private readonly ILocalStorage _storage;

    public AuthService(IAuthApi api, ILocalStorage storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<AuthResponse?> Login(LoginRequest request)
    {
        try
        {
            var response = await _api.Login(request);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                await _storage.SetAsync("auth_token", response.Content.Token);
                await _storage.SetAsync("refresh_token", response.Content.RefreshToken);
                return response.Content;
            }

            return null;
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            Console.WriteLine($"Status Code: {ex.StatusCode}");
            Console.WriteLine($"Content: {ex.Content}");
            return null; // ✅ این خط را اضافه کنید
        }
    }

    public async Task<bool> Logout()
    {
        try
        {
            await _api.Logout();
            await _storage.RemoveAsync("auth_token");
            await _storage.RemoveAsync("refresh_token");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetToken()
    {
        return await _storage.GetAsync<string>("auth_token");
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await GetToken();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<AuthResponse?> Refresh(RefreshRequest request)
    {
        try
        {
            var response = await _api.Refresh(request);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                await _storage.SetAsync("auth_token", response.Content.Token);
                await _storage.SetAsync("refresh_token", response.Content.RefreshToken);
                return response.Content;
            }

            return null;
        }
        catch (ApiException)
        {
            return null;
        }
    }
}