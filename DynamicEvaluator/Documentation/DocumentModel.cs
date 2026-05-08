//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

public sealed class DocumentModel
{
    [JsonPropertyName("functions")]
    public required FunctionModel[] Functions { get; set; }
}
