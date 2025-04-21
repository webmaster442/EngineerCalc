namespace EngineerCalc.Calculator;

public sealed class TableData
{
    public List<string> HeaderColumns { get; }
    public List<string[]> TableContent { get; }

    public TableData()
    {
        HeaderColumns = [];
        TableContent = [];
    }
}
