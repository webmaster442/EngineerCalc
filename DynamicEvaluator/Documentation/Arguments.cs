using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class Arguments
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("description")]
    public required string Description { get; init; }
}
