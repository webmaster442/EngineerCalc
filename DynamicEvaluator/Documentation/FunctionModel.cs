//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class FunctionModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("summary")]
    public required string Summary { get; set; }

    [JsonPropertyName("parameters")]
    public required ParameterModel[] Parameters { get; set; }

    [JsonPropertyName("examples")]
    public required string[] Examples { get; set; }
}
