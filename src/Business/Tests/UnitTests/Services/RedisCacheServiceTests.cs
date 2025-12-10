using Bogus;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Tests.UnitTests.Services;

public class RedisCacheServiceTests
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisCacheService<string, TestValue> _cacheService;

    public RedisCacheServiceTests()
    {
        var options = Options.Create(new MemoryDistributedCacheOptions());
        _distributedCache = new MemoryDistributedCache(options);
        _cacheService = new RedisCacheService<string, TestValue>(_distributedCache);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = new Faker().Random.AlphaNumeric(10);

        // Act
        var result = await _cacheService.GetAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_And_GetAsync_ShouldStoreAndRetrieveValue()
    {
        // Arrange
        var faker = new Faker();
        var key = faker.Random.AlphaNumeric(10);
        var value = new TestValue { Value = faker.Random.AlphaNumeric(20) };

        // Act
        await _cacheService.SetAsync(key, value);
        var result = await _cacheService.GetAsync(key);

        // Assert
        result.Should().BeEquivalentTo(value);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValue()
    {
        // Arrange
        var faker = new Faker();
        var key = faker.Random.AlphaNumeric(10);
        var value = new TestValue { Value = faker.Random.AlphaNumeric(20) };
        await _cacheService.SetAsync(key, value);

        // Act
        await _cacheService.RemoveAsync(key);
        var result = await _cacheService.GetAsync(key);

        // Assert
        result.Should().BeNull();
    }

    public class TestValue
    {
        public string? Value { get; set; }
    }
}
