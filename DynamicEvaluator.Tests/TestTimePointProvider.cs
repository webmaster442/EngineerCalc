using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

internal sealed class TestTimePointProvider : ITimePointProvider
{
    public int ClockDriftInSeconds => 0;

    public DateTime UtcNow()
    {
        return new DateTime(2026, 9,  17, 19, 0, 0, DateTimeKind.Utc);
    }
}
