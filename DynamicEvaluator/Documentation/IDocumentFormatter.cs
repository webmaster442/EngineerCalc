//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Documentation;

public interface IDocumentFormatter
{
    string FormatName(string name);
    string FormatSummary(string summary);
    string FormatSectionTitle(string title);
    string FormatExample(string example);
    string FormatDescription(string description);
    string FormatTypes(string[] types);
}
