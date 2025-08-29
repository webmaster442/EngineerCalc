using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicEvaluator.VariableSerialization;

internal sealed class PrecisionAccurateDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString() 
            ?? throw new InvalidOperationException("Value can't be null or empty");

        return double.TryParse(value, CultureInfo.InvariantCulture, out double result)
            ? result
            : throw new InvalidOperationException($"Value '{value}' is not a valid double.");
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("G17", CultureInfo.InvariantCulture));
    }
}
