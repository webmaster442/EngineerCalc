//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace EngineerCalc.Tui.Readline;

internal interface IHistory
{
    int Count { get; }
    string Current { get; }
    int CurrentIndex { get; set; }
    List<string> Entries { get; }
    void Add(string entry);
    void ResetIndex();
}
