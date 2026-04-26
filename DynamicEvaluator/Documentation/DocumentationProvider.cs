//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Text.Json;

namespace DynamicEvaluator.Documentation;

public sealed class DocumentationProvider : IEnumerable<Docmodel>
{
    private readonly Dictionary<string, Docmodel> _documents;

    public DocumentationProvider()
    {
        using var stream = typeof(DocumentationProvider).Assembly.GetManifestResourceStream("DynamicEvaluator.Documentation.documentation.json");
        if (stream == null)
        {
            throw new InvalidOperationException("documentation.json is not embedded");
        }
        var loaded = JsonSerializer.Deserialize<Docmodel[]>(stream, JsonSerializerOptions.Web);
        if (loaded == null)
        {
            throw new InvalidOperationException("Failed to deserialize documentation.json");
        }
        _documents = loaded.ToDictionary(d => d.FunctionName, d => d, StringComparer.InvariantCultureIgnoreCase);
    }

    public IEnumerable<string> FunctionNames
        => _documents.Keys;

    public Docmodel GetDocumentation(string function)
        => _documents[function];

    public IEnumerator<Docmodel> GetEnumerator()
        => _documents.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
