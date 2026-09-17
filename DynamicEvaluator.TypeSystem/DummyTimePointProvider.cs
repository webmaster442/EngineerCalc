namespace DynamicEvaluator.TypeSystem;

public sealed class DummyTimePointProvider : ITimePointProvider
{
    public int ClockDriftInSeconds => 0;

    public DateTime UtcNow()
        => new DateTime(0, DateTimeKind.Utc);
}
