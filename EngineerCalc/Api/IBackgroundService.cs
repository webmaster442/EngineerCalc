namespace EngineerCalc.Api;

internal interface IBackgroundService
{
    TimeSpan TriggerInterval { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
