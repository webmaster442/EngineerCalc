//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Domain;

internal interface IBackgroundService
{
    TimeSpan TriggerInterval { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
