//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Domain;

internal interface IApplicationApi
{
    void Exit(int exitCode);
    void Clear();
}
