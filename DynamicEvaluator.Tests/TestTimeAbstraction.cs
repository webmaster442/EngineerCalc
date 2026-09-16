using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

internal sealed class TestTimeAbstraction : ITimeAbstraction
{
    private readonly DateTimeOffset _testDate 
        = new DateTimeOffset(2025, 9, 16, 12, 0, 0, TimeSpan.Zero);

    public ValueTask<DateTimeOffset> GetNetworkUtcNow()
        => ValueTask.FromResult(_testDate);

    public ValueTask<DateTimeOffset> GetNetworkNow()
        => ValueTask.FromResult(_testDate);

    public DateTimeOffset GetUtcNow()
        => _testDate;

    public DateTimeOffset GetNow()
        => _testDate;
}
