//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace DynamicEvaluator;

public sealed class DocumentationProvider
{
    private readonly Dictionary<string, string> _documentation;

    public DocumentationProvider()
    {
        _documentation = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        using var stream = typeof(DocumentationProvider).Assembly.GetManifestResourceStream("DynamicEvaluator.Doc.md");
        if (stream == null)
        {
            throw new InvalidOperationException("Doc.md is not embedded");
        }
        using var reader = new StreamReader(stream);

        StringBuilder currentDoc = new(512);
        string? currentLine = null;
        string currentChapter = "";
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (currentLine.StartsWith("# "))
            {
                if (currentDoc.Length > 0)
                {
                    _documentation.Add(currentChapter, currentDoc.ToString());
                    currentDoc.Clear();
                    currentChapter = currentLine[1..].Trim();
                }
                else
                {
                    currentChapter = currentLine[1..].Trim();
                }
            }
            else
            {
                currentDoc.AppendLine(currentLine);
            }
        }
        if (currentDoc.Length > 0)
        {
            _documentation.Add(currentChapter, currentDoc.ToString());
        }
    }

    public IEnumerable<string> FunctionNames
        => _documentation.Keys;

    public string GetDocumentation(string function)
        => _documentation[function];
}
