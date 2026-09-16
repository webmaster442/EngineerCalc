using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

internal sealed class TestTimeAbstraction : ITimeAbstraction
{
    private readonly DateTimeOffset _testDate = new DateTimeOffset(2025, 9, 16, 12, 0, 0, TimeSpan.Zero);

    public ValueTask<DateTimeOffset> GetNetworkUtcNow()
    {
        return ValueTask.FromResult(_testDate);
    }

    public DateTimeOffset GetUtcNow()
    {
        return _testDate;
    }
}
