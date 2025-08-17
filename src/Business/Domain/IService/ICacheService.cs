namespace Domain.IService;

public interface ICacheService<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    /// <summary>
    /// Gets a value from the cache.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The cached value, or null if not found.</returns>
    Task<TValue?> GetAsync(TKey key);

    /// <summary>
    /// Sets a value in the cache.
    /// </summary>
    /// <param name="key">The key of the value to set.</param>
    /// <param name="value">The value to cache.</param>
    Task SetAsync(TKey key, TValue value);

    /// <summary>
    /// Removes a value from the cache.
    /// </summary>
    /// <param name="key">The key of the value to remove.</param>
    Task RemoveAsync(TKey key);
}
