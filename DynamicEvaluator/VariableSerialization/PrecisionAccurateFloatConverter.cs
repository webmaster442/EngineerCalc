using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicEvaluator.VariableSerialization;

internal sealed class PrecisionAccurateFloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString() 
            ?? throw new InvalidOperationException("Value can't be null or empty");

        return float.TryParse(value, CultureInfo.InvariantCulture, out float result)
            ? result
            : throw new InvalidOperationException($"Value '{value}' is not a valid float.");
    }
    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("G9", CultureInfo.InvariantCulture));
    }
}
