//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.TypeSystem;

public sealed class DummyTimePointProvider : ITimePointProvider
{
    public int ClockDriftInSeconds => 0;

    public DateTime UtcNow()
        => new DateTime(0, DateTimeKind.Utc);
}
