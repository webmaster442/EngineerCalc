using System.Net;
using System.Net.Sockets;

namespace DynamicEvaluator;

/// <summary>
/// A <see cref="TimeProvider"/> implementation that obtains the current UTC time
/// from an NTP (Network Time Protocol) server.
/// </summary>
public sealed class NtpTimeProvider : TimeProvider
{
    private const string DefaultNtpServer = "pool.ntp.org";
    private const int NtpPort = 123;
    private const int NtpPacketSize = 48;

    // Number of seconds between 1 Jan 1900 (NTP epoch) and 1 Jan 1970 (Unix epoch).
    private const long NtpToUnixEpochSeconds = 2208988800L;

    private readonly string _ntpServer;
    private readonly TimeSpan _timeout;

    /// <summary>
    /// Initializes a new instance of the <see cref="NtpTimeProvider"/> class.
    /// </summary>
    /// <param name="ntpServer">
    /// The host name or IP address of the NTP server to query. Defaults to "pool.ntp.org".
    /// </param>
    /// <param name="timeout">
    /// The maximum time to wait for a response from the server. Defaults to 5 seconds.
    /// </param>
    public NtpTimeProvider(string ntpServer = DefaultNtpServer, TimeSpan? timeout = null)
    {
        if (string.IsNullOrWhiteSpace(ntpServer))
            throw new ArgumentException("NTP server must not be null or empty.", nameof(ntpServer));

        _ntpServer = ntpServer;
        _timeout = timeout ?? TimeSpan.FromSeconds(5);
    }

    /// <summary>
    /// Gets the name or address of the configured NTP server.
    /// </summary>
    public string NtpServer => _ntpServer;

    /// <inheritdoc />
    public override DateTimeOffset GetUtcNow()
    {
        return QueryNtpServer();
    }

    private DateTimeOffset QueryNtpServer()
    {
        // NTP request packet. The first byte sets:
        // Leap Indicator = 0, Version Number = 3, Mode = 3 (client).
        byte[] ntpData = new byte[NtpPacketSize];
        ntpData[0] = 0x1B;

        IPAddress[] addresses = Dns.GetHostAddresses(_ntpServer);
        if (addresses.Length == 0)
            throw new InvalidOperationException($"Could not resolve NTP server '{_ntpServer}'.");

        var endPoint = new IPEndPoint(addresses[0], NtpPort);

        using var socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        socket.ReceiveTimeout = (int)_timeout.TotalMilliseconds;
        socket.SendTimeout = (int)_timeout.TotalMilliseconds;

        socket.Connect(endPoint);
        socket.Send(ntpData);
        socket.Receive(ntpData);

        // The transmit timestamp starts at byte offset 40 and consists of a
        // 32-bit seconds part followed by a 32-bit fractional part.
        ulong intPart = ((ulong)ntpData[40] << 24) | ((ulong)ntpData[41] << 16)
                      | ((ulong)ntpData[42] << 8) | ntpData[43];

        ulong fractPart = ((ulong)ntpData[44] << 24) | ((ulong)ntpData[45] << 16)
                        | ((ulong)ntpData[46] << 8) | ntpData[47];

        double milliseconds = ((long)intPart - NtpToUnixEpochSeconds) * 1000.0
                            + (fractPart * 1000.0 / 0x100000000L);

        return DateTimeOffset.UnixEpoch.AddMilliseconds(milliseconds);
    }
}
