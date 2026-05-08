using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class ParameterModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("supportedTypes")]
    public required string SupportedTypes { get; set; }
}
