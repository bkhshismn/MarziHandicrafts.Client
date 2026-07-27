using Marzi.Client.Core.Interfaces;
using Microsoft.JSInterop;

namespace Marzi.Client.Infrastructure.Storage;

public class LocalStorage : ILocalStorage
{
    private readonly IJSRuntime _js;

    public LocalStorage(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetAsync<T>(string key, T value)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(value);
        await _js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(json))
            return default;

        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync(string key)
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", key);
    }
}