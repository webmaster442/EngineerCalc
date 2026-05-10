//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace DynamicEvaluator.Documentation;

public sealed class DocumentationRenderer 
{
    private readonly IDocumentFormatter _formatter;
    private readonly DocumentationProvider _documentation;

    public DocumentationRenderer(IDocumentFormatter formatter, DocumentationProvider documentation)
    {
        _formatter = formatter;
        _documentation = documentation;
    }

    public string GetDocumentation(string functionName)
    {
        var model = _documentation.GetDocumentation(functionName);
        StringBuilder buffer = new(1024);

        buffer
            .AppendLine(_formatter.FormatName(model.Name))
            .AppendLine()
            .AppendLine(_formatter.FormatSummary(model.Summary))
            .AppendLine();

        buffer
            .AppendLine(_formatter.FormatSectionTitle("Examples"))
            .AppendLine();

        foreach (var example in model.Examples)
        {
            buffer.AppendLine(_formatter.FormatExample(example));
        }
        buffer
            .AppendLine()
            .AppendLine(_formatter.FormatSectionTitle("Parameters"))
            .AppendLine();

        foreach (var parameter in model.Parameters)
        {
            buffer
                .AppendLine(_formatter.FormatExample(parameter.Name))
                .AppendLine(_formatter.FormatDescription(parameter.Description))
                .AppendLine(_formatter.FormatTypes(parameter.SupportedTypes.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));
                
        }

        return buffer.ToString();

    }
}
