//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Tui.Readline;

internal sealed class InMemoryHistoryList : IHistory
{
    public List<string> Entries { get; }
    public int CurrentIndex { get; set; }

    public InMemoryHistoryList()
    {
        Entries = new List<string>();
        CurrentIndex = Entries.Count - 1;
    }

    public void ResetIndex()
        => CurrentIndex = Entries.Count - 1;

    public void Add(string entry)
    {
        if (string.IsNullOrWhiteSpace(entry))
            return;

        Entries.Add(entry);
    }

    public string Current => Entries[CurrentIndex];

    public int Count => Entries.Count;
}
