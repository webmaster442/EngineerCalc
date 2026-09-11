namespace EngineerCalc.Domain;

internal interface IBackgroundService
{
    TimeSpan TriggerInterval { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
