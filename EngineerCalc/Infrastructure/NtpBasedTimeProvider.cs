//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;
using System.Net.Sockets;

using DynamicEvaluator.TypeSystem;

using Microsoft.Extensions.Logging;

namespace EngineerCalc.Infrastructure;

public sealed class NtpBasedTimeProvider : ITimePointProvider
{
    private readonly Lock _lock;
    private TimeSpan _drift;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<NtpBasedTimeProvider> _logger;

    public NtpBasedTimeProvider(TimeProvider timeProvider, ILoggerFactory loggerFactory)
    {
        _lock = new Lock();
        _drift = TimeSpan.Zero;
        _timeProvider = timeProvider;
        _logger = loggerFactory.CreateLogger<NtpBasedTimeProvider>();
    }

    public int ClockDriftInSeconds
    {
        get
        {
            lock (_lock)
            {
                return (int)_drift.TotalSeconds;
            }
        }
    }

    public DateTime UtcNow()
    {
        lock (_lock)
        {
            return _timeProvider.GetUtcNow().UtcDateTime + _drift;
        }
    }

    public async ValueTask Update(string ntpServer = "pool.ntp.org", int ntpPort = 123)
    {
        var ntpTime = await TryFetchNtpTime(ntpServer, ntpPort, 1000);
        
        if (ntpTime.HasValue)
        {
            lock (_lock)
            {
                var localTime = _timeProvider.GetUtcNow().UtcDateTime;
                var calculatedDrift = ntpTime.Value - localTime;
                _logger.LogInformation("NTP time fetched. Calculated drift: {CalculatedDrift}", calculatedDrift.TotalSeconds);
                _drift = calculatedDrift.TotalSeconds > 0 ? calculatedDrift : TimeSpan.Zero;
            }
        }
    }

    private async ValueTask<DateTime?> TryFetchNtpTime(string ntpServer, int ntpPort, int timeout)
    {
        using var cancellationTokenSource = new CancellationTokenSource(timeout);
        try
        {
            // NTP request packet is 48 bytes; first byte: LI = 0, VN = 3, Mode = 3 (client).
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            IPAddress[] addresses = await Dns.GetHostAddressesAsync(ntpServer, cancellationTokenSource.Token);
            if (addresses.Length == 0)
            {
                return null;
            }

            var endPoint = new IPEndPoint(addresses[0], ntpPort);

            using var socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            await socket.ConnectAsync(endPoint, cancellationTokenSource.Token);
            await socket.SendAsync(ntpData, SocketFlags.None, cancellationTokenSource.Token);
            await socket.ReceiveAsync(ntpData, SocketFlags.None, cancellationTokenSource.Token);

            // The transmit timestamp starts at byte 40 and is an 8-byte value:
            // 4 bytes of seconds followed by 4 bytes of fractional seconds since 1900-01-01.
            uint intPart = (uint)((ntpData[40] << 24) | (ntpData[41] << 16) | (ntpData[42] << 8) | ntpData[43]);
            uint fractPart = (uint)((ntpData[44] << 24) | (ntpData[45] << 16) | (ntpData[46] << 8) | ntpData[47]);

            ulong milliseconds = (intPart * 1000UL) + ((fractPart * 1000UL) / 0x100000000UL);

            var networkDateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(milliseconds);

            return networkDateTime;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("NTP request to {NtpServer}:{NtpPort} timed out.", ntpServer, ntpPort);
            return null;
        }
    }
}
