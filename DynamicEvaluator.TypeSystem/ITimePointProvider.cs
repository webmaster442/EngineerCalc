//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.TypeSystem;

public interface ITimePointProvider
{
    DateTime UtcNow();

    int ClockDriftInSeconds { get; }
}
