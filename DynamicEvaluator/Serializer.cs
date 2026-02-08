//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;

using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public static class Serializer
{
    public static string ToJson(this VariablesAndConstantsCollection variables)
    {
        JsonObject root = new JsonObject();
        foreach (var (name, variable) in variables.Variables())
        {
            root.Add(name, CreateNode(variable));
        }
        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    public static void FromJson(this VariablesAndConstantsCollection variables, string json, bool clearExisting = false)
    {
        if (clearExisting)
            variables.Clear();

        JsonObject? root = JsonNode.Parse(json) as JsonObject
            ?? throw new InvalidOperationException("Invalid JSON format.");

        foreach (var (name, node) in root)
        {
            if (node is JsonValue jsonValue)
            {
                variables.Add(name, GetValue(jsonValue));
            }
            else if (node is JsonObject jsonObject)
            {
                variables.Add(name, GetComplexType(jsonObject));
            }
            else
            {
                throw new InvalidOperationException("Unsupported JSON node type.");
            }
        }
    }

    private static dynamic GetComplexType(JsonObject obj)
    {
        static T GetValue<T>(JsonObject jsonObject, string key) where T : notnull
        {
            var value = jsonObject[key];
            return value == null
                ? throw new InvalidOperationException($"Missing {key} property has null value")
                : value.GetValue<T>()
                ?? throw new InvalidOperationException($"Missing {key} property");
        }

        var type = obj["Type"]?.GetValue<string>()
            ?? throw new InvalidOperationException("Missing 'Type' property in complex JSON object.");

        return type switch
        {
            "Complex" => new Complex(GetValue<double>(obj, nameof(Complex.Real)),
                                     GetValue<double>(obj, nameof(Complex.Imaginary))),
            "Vector2" => new Vector2(GetValue<float>(obj, nameof(Vector2.X)),
                                     GetValue<float>(obj, nameof(Vector2.Y))),
            "Vector3" => new Vector3(GetValue<float>(obj, nameof(Vector3.X)),
                                     GetValue<float>(obj, nameof(Vector3.Y)),
                                     GetValue<float>(obj, nameof(Vector3.Z))),
            "Vector4" => new Vector4(GetValue<float>(obj, nameof(Vector4.X)),
                                     GetValue<float>(obj, nameof(Vector4.Y)),
                                     GetValue<float>(obj, nameof(Vector4.Z)),
                                     GetValue<float>(obj, nameof(Vector4.W))),
            "Fraction" => new Fraction(GetValue<long>(obj, nameof(Fraction.Numerator)),
                                       GetValue<long>(obj, nameof(Fraction.Denominator))),
            "ValueUnit" => new ValueUnit(GetValue<double>(obj, nameof(ValueUnit.Value)),
                                         GetValue<string>(obj, nameof(ValueUnit.Unit))),
            _ => throw new InvalidOperationException("Unknown type")
        };
    }

    private static dynamic GetValue(JsonValue jsonValue)
    {
        if (jsonValue.TryGetValue<byte>(out var b)) return b;
        if (jsonValue.TryGetValue<sbyte>(out var sb)) return sb;
        if (jsonValue.TryGetValue<short>(out var s)) return s;
        if (jsonValue.TryGetValue<ushort>(out var us)) return us;
        if (jsonValue.TryGetValue<int>(out var i)) return i;
        if (jsonValue.TryGetValue<uint>(out var ui)) return ui;
        if (jsonValue.TryGetValue<long>(out var l)) return l;
        if (jsonValue.TryGetValue<ulong>(out var ul)) return ul;
        if (jsonValue.TryGetValue<float>(out var f)) return f;
        if (jsonValue.TryGetValue<double>(out var d)) return d;
        if (jsonValue.TryGetValue<decimal>(out var dec)) return dec;
        if (jsonValue.TryGetValue<string>(out var str)) return str;
        throw new InvalidOperationException("Unsupported JSON value type.");
    }

    private static JsonNode CreateNode(dynamic variable)
    {
        return variable switch
        {
            byte b => JsonValue.Create(b),
            sbyte sb => JsonValue.Create(sb),
            short s => JsonValue.Create(s),
            ushort us => JsonValue.Create(us),
            int i => JsonValue.Create(i),
            uint ui => JsonValue.Create(ui),
            long l => JsonValue.Create(l),
            ulong ul => JsonValue.Create(ul),
            float f => JsonValue.Create(f),
            double d => JsonValue.Create(d),
            decimal dec => JsonValue.Create(dec),
            string str => JsonValue.Create(str),
            Complex c => new JsonObject
            {
                { "Type", "Complex" },
                { "Real", c.Real },
                { "Imaginary", c.Imaginary }
            },
            Vector2 v2 => new JsonObject
            {
                { "Type", "Vector2" },
                { "X", v2.X },
                { "Y", v2.Y }
            },
            Vector3 v3 => new JsonObject
            {
                { "Type", "Vector3" },
                { "X", v3.X },
                { "Y", v3.Y },
                { "Z", v3.Z }
            },
            Vector4 v4 => new JsonObject
            {
                { "Type", "Vector4" },
                { "X", v4.X },
                { "Y", v4.Y },
                { "Z", v4.Z },
                { "W", v4.W }
            },
            Fraction frac => new JsonObject
            {
                { "Type", "Fraction" },
                { "Numerator", frac.Numerator },
                { "Denominator", frac.Denominator }
            },
            ValueUnit valueUnit => new JsonObject
            {
                { "Type", "ValueUnit" },
                { "Value", valueUnit.Value },
                { "Unit", valueUnit.Unit }
            },
            _ => throw new InvalidOperationException($"Unsupported type: {variable.GetType()}"),
        };
    }
}
