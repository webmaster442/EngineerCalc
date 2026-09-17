//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Domain;

using Microsoft.Extensions.Logging;

namespace EngineerCalc.Infrastructure;

internal sealed class BackgroundServiceRunner : IDisposable
{
    private readonly IBackgroundService[] _services;
    private readonly DateTime[] _nextRuns;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger<BackgroundServiceRunner> _logger;

    public BackgroundServiceRunner(IBackgroundService[] services, ILoggerFactory loggerFactory)
    {
        _services = services;
        _nextRuns = new DateTime[services.Length];
        _cancellationTokenSource = new CancellationTokenSource();
        _logger = loggerFactory.CreateLogger<BackgroundServiceRunner>();

        for (int i = 0; i < services.Length; i++)
        {
            _nextRuns[i] = DateTime.UtcNow + services[i].TriggerInterval;
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public async void Start()
    {
        await Run();
    }

    private const int Timeout = 60_000;

    private async Task Run()
    {
        foreach (var service in _services)
        {
            // Run each service once at startup
            using var cts = new CancellationTokenSource(Timeout);
            using var runner = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cts.Token);
            try
            {
                await service.ExecuteAsync(runner.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Service {ServiceName} exceeded the timeout of {Timeout}ms during startup.", service.GetType().Name, Timeout);
                // Service exceeded the timeout; move on to the next service
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing service {ServiceName} during startup.", service.GetType().Name);
                // Swallow exceptions to keep the loop alive; individual services are responsible for their own logging)
            }
        }

        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            for (int i = 0; i < _services.Length; i++)
            {
                if (DateTime.UtcNow >= _nextRuns[i])
                {
                    using var cts = new CancellationTokenSource(Timeout);
                    using var runner = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cts.Token);

                    try
                    {
                        await _services[i].ExecuteAsync(runner.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Service exceeded the timeout; move on to the next service
                        _logger.LogWarning("Service {ServiceName} exceeded the timeout of {Timeout}ms.", _services[i].GetType().Name, Timeout);
                    }
                    catch (Exception ex)
                    {
                        // Swallow exceptions to keep the loop alive; individual services are responsible for their own logging)
                        _logger.LogError(ex, "An error occurred while executing service {ServiceName}.", _services[i].GetType().Name);
                    }

                    _nextRuns[i] = DateTime.UtcNow + _services[i].TriggerInterval;

                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Service {ServiceName} executed successfully.", _services[i].GetType().Name);
                    }
                }
            }
        }
    }
}
