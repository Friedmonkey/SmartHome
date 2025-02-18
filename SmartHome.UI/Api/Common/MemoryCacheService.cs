using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CacheObject = (System.DateTime cacheTime, System.Type type, object? value);

namespace SmartHome.UI.Api;

public class MemoryCacheService
{
    public MemoryCacheService()
    {
        innerCache = new();
    }
    private readonly Dictionary<string, CacheObject> innerCache;

    public void Set<T>(string cacheKey, T value)
    {
        innerCache.Add(cacheKey, (DateTime.UtcNow, typeof(T), value));
        Console.WriteLine(cacheKey + " was added to cache");
    }
    public void RemoveCache(string cacheKey)
    {   //specifically remove an single cache entry that must exactly match
        if (innerCache.Remove(cacheKey))
            Console.WriteLine(cacheKey + " was removed from cache");
    }
    public void RemoveCacheWithPrimary(string primaryKey)
    {   //clear a specific type of cache
        var baseKey = Tobase64(primaryKey);
        foreach (var cacheKey in innerCache.Keys) 
        {
            if (cacheKey.StartsWith(baseKey + '.'))
            {
                if (innerCache.Remove(cacheKey))
                    Console.WriteLine(cacheKey + " was removed from cache");
            }
        }
    }
    public void FullyClearCache()
    {
        innerCache.Clear();
        Console.WriteLine("cache was fully cleared");
    }

    public bool TryGet<T>(string cacheKey, TimeSpan maxAge, out T value)
    {
        if (innerCache.TryGetValue(cacheKey, out CacheObject entry))
        {
            if (entry.value is null)
                throw new Exception("Cache value was null");
            if (entry.type != typeof(T))
                throw new Exception($"Cache type mismatch, expected {typeof(T)} but cache was {entry.type}");
            if (DateTime.UtcNow - entry.cacheTime <= maxAge)
            {
                value = (T)entry.value;
                return true;
            }
            else
            {   //cache is expired so lets remove it
                innerCache.Remove(cacheKey);
            }
        }
        value = default!;
        return false;
    }

    public string HashKey(string primary, object? secondary)
    {
        var combined = (secondary != null ? JsonSerializer.Serialize(secondary) : "");
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Tobase64(primary) +'.'+ Convert.ToBase64String(bytes);
    }
    public string Tobase64(string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }
}
