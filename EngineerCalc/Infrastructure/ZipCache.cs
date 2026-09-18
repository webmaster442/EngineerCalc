using System.IO.Compression;
using System.Text;
using System.Text.Json;

using EngineerCalc.Domain;

using Microsoft.Extensions.Configuration;

namespace EngineerCalc.Infrastructure;

internal sealed class ZipCache : IRemoteApiCache, IDisposable
{
    private readonly ZipArchive _archive;
    private readonly TimeProvider _provider;
    private readonly Lock _lock;
    private bool _disposed;
   
    public ZipCache(TimeProvider provider)
    {
        var cachefilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "engineercalc.cache");
        _archive = new ZipArchive(File.Open(cachefilePath, FileMode.OpenOrCreate), ZipArchiveMode.Update);
        _provider = provider;
        _lock = new Lock();
    }

    public void ClearExpired()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        lock (_lock)
        {
            var now = _provider.GetUtcNow();
            foreach (var entry in _archive.Entries)
            {
                if (entry.Comment.Length == 0)
                {
                    continue;
                }
                var expiryDate = JsonSerializer.Deserialize<DateTime>(entry.Comment, JsonSerializerOptions.Web);
                if (expiryDate < now)
                {
                    entry.Delete();
                }
            }
        }
    }

    public void Dispose()
    {
        _archive.Dispose();
        _disposed = true;
    }

    private static string CreateEntryName(string key)
    {
        ulong hash = 0xcbf29ce484222325;
        foreach (var chr in key)
        {
            hash = (hash ^ chr) * 0x00000100000001b3Ul;
        }
        return Convert.ToBase64String(BitConverter.GetBytes(hash));
    }

    public void Store(string key, string value, DateTime expiryDate)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        lock (_lock)
        {
            var entry = _archive.GetEntry(CreateEntryName(key));
            entry?.Delete();
            entry = _archive.CreateEntry(CreateEntryName(key));
            entry.Comment = JsonSerializer.Serialize(expiryDate, JsonSerializerOptions.Web);
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(value);
        }
    }

    public void Store(string key, string value, TimeSpan timeToLive)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        DateTime targetDate = _provider.GetUtcNow().Add(timeToLive).UtcDateTime;
        Store(key, value, targetDate);
    }

    public bool TryGet(string key, out string value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        lock (_lock)
        {
            var entry = _archive.GetEntry(CreateEntryName(key));
            if (entry == null)
            {
                value = null!;
                return false;
            }
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            value = reader.ReadToEnd();
            return true;
        }
    }
}
