namespace solidcode.work.infra.Redis;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
}
