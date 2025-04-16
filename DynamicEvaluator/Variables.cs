using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DynamicEvaluator;

public sealed class Variables : IEnumerable<KeyValuePair<string, object>>
{
    private readonly Dictionary<string, dynamic> _variables;

    public Variables()
    {
        _variables = new Dictionary<string, dynamic>()
        {
            { "e", Math.E },
            { "pi", Math.PI },
            { "tau", Math.Tau },
        };
    }

    public void Add(string key, dynamic value)
    {
        var keyToUse = key.ToLower();
        _variables.Add(keyToUse, value);
    }

    public dynamic this[string key]
        => _variables[key];

    public IEnumerable<string> Keys => _variables.Keys;
    
    public IEnumerable<dynamic> Values => _variables.Values;
    
    public int Count => _variables.Count;

    public bool ContainsKey(string key)
        => _variables.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
        => _variables.GetEnumerator();

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out dynamic value)
        => _variables.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
