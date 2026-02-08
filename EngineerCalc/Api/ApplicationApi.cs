//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Api;

public sealed class ApplicationApi : IApplicationApi
{
    public void Exit(int exitCode)
    {
        Environment.Exit(exitCode);
    }
    public void Clear()
    {
        Console.Clear();
    }
}
