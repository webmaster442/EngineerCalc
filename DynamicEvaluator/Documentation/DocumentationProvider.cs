//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Text.Json;

namespace DynamicEvaluator.Documentation;

public sealed class DocumentationProvider : IEnumerable<FunctionModel>
{
    private readonly Dictionary<string, FunctionModel> _documents;

    public DocumentationProvider()
    {
        using var stream = typeof(DocumentationProvider).Assembly.GetManifestResourceStream("DynamicEvaluator.Documentation.documentation.json");
        if (stream == null)
        {
            throw new InvalidOperationException("documentation.json is not embedded");
        }
        var loaded = JsonSerializer.Deserialize<DocumentModel>(stream, DocumentModelJsonSerializerContext.Default.DocumentModel);
        if (loaded == null)
        {
            throw new InvalidOperationException("Failed to deserialize documentation.json");
        }
        _documents = loaded.Functions.ToDictionary(d => d.Name, d => d, StringComparer.InvariantCultureIgnoreCase);
    }

    public IEnumerable<string> FunctionNames
        => _documents.Keys;

    public bool IsDocumented(string name)
        => _documents.ContainsKey(name);

    public FunctionModel GetDocumentation(string function)
        => _documents[function];

    public IEnumerator<FunctionModel> GetEnumerator()
        => _documents.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
