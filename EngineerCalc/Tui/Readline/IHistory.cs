
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
