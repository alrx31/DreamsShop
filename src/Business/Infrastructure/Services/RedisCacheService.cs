using System.Text.Json;
using Domain.IService;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services;

public class RedisCacheService<TKey, TValue>(IDistributedCache database) : ICacheService<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    public async Task<TValue?> GetAsync(TKey key)
    {
        var data = await database.GetStringAsync(JsonSerializer.Serialize(key));
        return data == null ? default : JsonSerializer.Deserialize<TValue>(data);
    }

    public async Task SetAsync(TKey key, TValue value)
    {
        await database.SetStringAsync(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value));
    }

    public async Task RemoveAsync(TKey key)
    {
        await database.RemoveAsync(JsonSerializer.Serialize(key));
    }
}
