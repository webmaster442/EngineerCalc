using System.Net;
using System.Net.Sockets;

namespace DynamicEvaluator.TypeSystem;

public sealed class TimeAbstraction : ITimeAbstraction
{
    private const string DefaultNtpServer = "pool.ntp.org";
    private const int NtpPort = 123;

    private readonly TimeProvider _localTimeProvider;
    private readonly int _networkTimeout;
    private readonly string _ntpServer;

    public TimeAbstraction(TimeProvider localTimeProvider,
                           int networkTimeout,
                           string ntpServer = DefaultNtpServer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(networkTimeout);
        ArgumentException.ThrowIfNullOrWhiteSpace(ntpServer);
        _localTimeProvider = localTimeProvider;
        _networkTimeout = networkTimeout;
        _ntpServer = ntpServer;
    }

    public async ValueTask<DateTimeOffset> GetNetworkUtcNow()
    {
        using var cancellationTokenSource = new CancellationTokenSource(_networkTimeout);
        try
        {
            // NTP request packet is 48 bytes; first byte: LI = 0, VN = 3, Mode = 3 (client).
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            IPAddress[] addresses = await Dns.GetHostAddressesAsync(_ntpServer, cancellationTokenSource.Token);
            if (addresses.Length == 0)
            {
                throw new InvalidOperationException($"Could not resolve NTP server '{_ntpServer}'.");
            }

            var endPoint = new IPEndPoint(addresses[0], NtpPort);

            using var socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            await socket.ConnectAsync(endPoint, cancellationTokenSource.Token);
            await socket.SendAsync(ntpData, SocketFlags.None, cancellationTokenSource.Token);
            await socket.ReceiveAsync(ntpData, SocketFlags.None, cancellationTokenSource.Token);

            // The transmit timestamp starts at byte 40 and is an 8-byte value:
            // 4 bytes of seconds followed by 4 bytes of fractional seconds since 1900-01-01.
            uint intPart = (uint)((ntpData[40] << 24) | (ntpData[41] << 16) | (ntpData[42] << 8) | ntpData[43]);
            uint fractPart = (uint)((ntpData[44] << 24) | (ntpData[45] << 16) | (ntpData[46] << 8) | ntpData[47]);

            ulong milliseconds = (intPart * 1000UL) + ((fractPart * 1000UL) / 0x100000000UL);

            var networkDateTime = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero)
                .AddMilliseconds(milliseconds);

            return networkDateTime;
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException($"The operation to get network time exceeded the timeout of {_networkTimeout} milliseconds.");
        }
    }

    public DateTimeOffset GetUtcNow()
        => _localTimeProvider.GetUtcNow();
}
