//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(Docmodel[]))]
internal partial class DocModelContext : JsonSerializerContext
{
}

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
        var loaded = JsonSerializer.Deserialize<Docmodel[]>(stream, DocModelContext.Default.DocmodelArray);
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
