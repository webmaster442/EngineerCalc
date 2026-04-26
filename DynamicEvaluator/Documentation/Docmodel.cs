using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class Docmodel
{
    [JsonPropertyName("functionName")]
    public required string FunctionName { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("examples")]
    public required string[] Examples { get; init; }

    [JsonPropertyName("arguments")]
    public required Arguments[] Arguments { get; init; }
}
