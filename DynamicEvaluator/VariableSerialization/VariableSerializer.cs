using System.Text.Json;

namespace DynamicEvaluator.VariableSerialization;

internal sealed class VariableSerializer
{
    private readonly JsonSerializerOptions _options;

    public VariableSerializer()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _options.WriteIndented = true;
        _options.Converters.Add(new PrecisionAccurateDoubleConverter());
        _options.Converters.Add(new PrecisionAccurateFloatConverter());
    }

    public string Save(VariablesAndConstantsCollection variables)
    {
        return JsonSerializer.Serialize(variables.Variables(), _options);
    }

    public void Restore(VariablesAndConstantsCollection target, string serializedVariables)
    {
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(serializedVariables, _options)
            ?? throw new InvalidOperationException("Deserialized variables can't be null");

        foreach (var (key, value) in deserialized)
        {
            target[key] = value;
        }
    }

}
