using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class DocumentModel
{
    [JsonPropertyName("functions")]
    public required FunctionModel[] Functions { get; set; }
}
