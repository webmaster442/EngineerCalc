using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EngineerCalc.Endpoints;

public class StateManager
{
    private readonly MemoryCache _memoryCache;

    public StateManager()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions
        {
            TrackStatistics = true,
        });
    }

    private static string GetId(IPAddress? remoteIpAddress, string scheme, StringValues userAgent)
    {
        if (remoteIpAddress == null)
            remoteIpAddress = new IPAddress([1, 1, 1, 1]);

        using MemoryStream pool = new MemoryStream(128);
        pool.Write(remoteIpAddress.GetAddressBytes());
        pool.Write(Encoding.UTF8.GetBytes(scheme));
        foreach (var agent in userAgent)
        {
            pool.Write(Encoding.UTF8.GetBytes(agent ?? ""));
        }
        pool.Seek(0, SeekOrigin.Begin);
        var data = SHA256.HashData(pool);
        return Convert.ToBase64String(data);
    }

    public delegate Task StateManipulator(State state);

    public async Task WithState(HttpRequest request, StateManipulator stateManipulation)
    {
        string id = GetId(request.HttpContext.Connection.RemoteIpAddress,
                          request.Scheme,
                          request.Headers.UserAgent);

        if (_memoryCache.TryGetValue(id, out State? state))
        {
            _memoryCache.Remove(id);
        }
        else
        {
            state = new State();
        }

        await stateManipulation.Invoke(state!);

        _memoryCache.Set(id, state, TimeSpan.FromMinutes(15));
    }
}
