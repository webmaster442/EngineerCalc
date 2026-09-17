namespace DynamicEvaluator.TypeSystem;

public interface ITimePointProvider
{
    DateTime UtcNow();

    int ClockDriftInSeconds { get; }
}
