using Marzi.Client.Core.Interfaces;
using Microsoft.Maui.Storage;

namespace Marzi.Client.Maui.Storage; // ✅ namespace درست

public class PreferencesStorage : ILocalStorage
{
    public async Task SetAsync<T>(string key, T value)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(value);
        Preferences.Set(key, json);
        await Task.CompletedTask;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = Preferences.Get(key, string.Empty);
        if (string.IsNullOrEmpty(json))
            return default;

        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync(string key)
    {
        Preferences.Remove(key);
        await Task.CompletedTask;
    }
}