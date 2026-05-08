namespace DynamicEvaluator.Documentation;

public interface IDocumentFormatter
{
    string FormatName(string name);
    string FormatSummary(string summary);
    string FormatSectionTitle(string title);
    string FormatExample(string example);
}
