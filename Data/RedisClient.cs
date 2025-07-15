using StackExchange.Redis;

namespace iworfShop_backend_light.Data;

public interface IRedisClient
{
    Task<string> GetValueAsync(string key);
    Task<string> SetValueAsync(string key, string value);
}

public class RedisClient :  IRedisClient
{
    private readonly IDatabase _redis;

    public RedisClient(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<string?> GetValueAsync(string key)
    {
        var value = await _redis.StringGetAsync(key);
        return value.IsNullOrEmpty ? "" : value;
    }

    public async Task<string> SetValueAsync(string key, string value)
    {
        try
        {
            await _redis.StringSetAsync(key, value);
            return value;
        }
        catch (Exception)
        {
            throw new Exception("Redis key could not be set");
        }
    }
}