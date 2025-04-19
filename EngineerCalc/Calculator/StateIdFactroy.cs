using System.Net;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Primitives;

namespace EngineerCalc.Calculator;

public class StateIdFactroy
{
    internal static string Create(IPAddress? remoteIpAddress, string scheme, StringValues userAgent)
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
}
