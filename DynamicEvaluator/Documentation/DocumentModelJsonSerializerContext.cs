//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicEvaluator.Documentation;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(DocumentModel))]
internal partial class DocumentModelJsonSerializerContext : JsonSerializerContext
{
}
