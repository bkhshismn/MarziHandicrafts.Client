namespace Marzi.Client.Core.Interfaces;

public interface ILocalStorage
{
    Task SetAsync<T>(string key, T value);

    Task<T?> GetAsync<T>(string key);

    Task RemoveAsync(string key);
}