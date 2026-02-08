//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Tui.Readline;

public interface ICompleter
{
    IEnumerable<string> GetCompletion(string line, int currentPosition);
}
