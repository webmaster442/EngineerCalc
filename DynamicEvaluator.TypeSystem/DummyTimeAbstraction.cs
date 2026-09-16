namespace DynamicEvaluator.TypeSystem;

public sealed class DummyTimeAbstraction : ITimeAbstraction
{
    public DateTimeOffset GetUtcNow()
        => DateTimeOffset.UtcNow;

    public ValueTask<DateTimeOffset> GetNetworkUtcNow()
        => new ValueTask<DateTimeOffset>(DateTimeOffset.UtcNow);
}
