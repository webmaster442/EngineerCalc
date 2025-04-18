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
            { "yocto", 1e-24 },
            { "zepto", 1e-21 },
            { "atto",  1e-18 },
            { "femto", 1e-15 },
            { "pico",  1e-12 },
            { "nano",  1e-9  },
            { "micro", 1e-6  },
            { "milli", 1e-3  },
            { "centi", 1e-2  },
            { "deci",  1e-1  },
            { "deca",  1e1   },
            { "hecto", 1e2   },
            { "kilo",  1e3   },
            { "mega",  1e6   },
            { "giga",  1e9   },
            { "tera",  1e12  },
            { "peta",  1e15  },
            { "exa",   1e18  },
            { "zetta", 1e21  },
            { "yotta", 1e24  },
            { "ronna", 1e27  },
            { "quetta",1e30  }
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
