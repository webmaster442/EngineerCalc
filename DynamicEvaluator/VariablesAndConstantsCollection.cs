//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Specialized;

namespace DynamicEvaluator;

public sealed class VariablesAndConstantsCollection : IEnumerable<KeyValuePair<string, object>>, INotifyCollectionChanged
{
    private readonly Dictionary<string, dynamic> _variables;
    private readonly Dictionary<string, dynamic> _constants;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public VariablesAndConstantsCollection()
    {
        _variables = new Dictionary<string, dynamic>();
        _constants = new Dictionary<string, dynamic>()
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

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item = null)
        => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));

    public bool IsConstant(string key)
        => _constants.ContainsKey(key);

    public bool IsVariable(string key)
        => _variables.ContainsKey(key);

    public bool IsDefined(string key)
        => IsConstant(key) || IsVariable(key);

    public void Add(string key, dynamic value)
    {
        if (_constants.ContainsKey(key))
            throw new InvalidOperationException($"{key} is a constant, that can't be overdefined");

        var keyToUse = key.ToLower();
        _variables[keyToUse] = value;
        OnCollectionChanged(NotifyCollectionChangedAction.Add, key);
    }

    public void Clear()
    {
        _variables.Clear();
        OnCollectionChanged(NotifyCollectionChangedAction.Reset);
    }

    public bool Remove(string key)
    {
        if (_variables.Remove(key))
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, key);
            return true;
        }
        return false;
    }

    public dynamic this[string key]
    {
        get => _constants.ContainsKey(key) ? _constants[key] : _variables[key];
        set => Add(key, value);
    }

    public IEnumerable<KeyValuePair<string, dynamic>> Variables()
    {
        foreach (var item in _variables)
            yield return item;
    }

    public IEnumerable<KeyValuePair<string, dynamic>> Constants()
    {
        foreach (var item in _constants)
            yield return item;
    }

    public IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
    {
        foreach (var item in _constants)
            yield return item;

        foreach (var item in _variables)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
