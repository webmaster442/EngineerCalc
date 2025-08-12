namespace EngineerCalc.Tui.Readline;

public interface ICompleter
{
    IEnumerable<string> GetCompletion(string line, int currentPosition);
}
