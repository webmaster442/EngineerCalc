//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Specialized;

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator;

public sealed class VariablesAndConstantsCollection : IEnumerable<KeyValuePair<string, Result>>, INotifyCollectionChanged
{
    private readonly Dictionary<string, Result> _variables;
    private readonly Dictionary<string, Result> _constants;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public VariablesAndConstantsCollection()
    {
        _variables = new Dictionary<string, Result>();
        _constants = new Dictionary<string, Result>()
        {
            { "e", Result.FromDouble(Math.E) },
            { "pi", Result.FromDouble(Math.PI) },
            { "tau", Result.FromDouble(Math.Tau) },
            { "yocto", Result.FromDouble(1e-24) },
            { "zepto", Result.FromDouble(1e-21) },
            { "atto",  Result.FromDouble(1e-18) },
            { "femto", Result.FromDouble(1e-15) },
            { "pico",  Result.FromDouble(1e-12) },
            { "nano",  Result.FromDouble(1e-9) },
            { "micro", Result.FromDouble(1e-6) },
            { "milli", Result.FromDouble(1e-3) },
            { "centi", Result.FromDouble(1e-2) },
            { "deci",  Result.FromDouble(1e-1) },
            { "deca",  Result.FromDouble(1e1) },
            { "hecto", Result.FromDouble(1e2) },
            { "kilo",  Result.FromDouble(1e3) },
            { "mega",  Result.FromDouble(1e6) },
            { "giga",  Result.FromDouble(1e9) },
            { "tera",  Result.FromDouble(1e12) },
            { "peta",  Result.FromDouble(1e15) },
            { "exa",   Result.FromDouble(1e18) },
            { "zetta", Result.FromDouble(1e21) },
            { "yotta", Result.FromDouble(1e24) },
            { "ronna", Result.FromDouble(1e27) },
            { "quetta", Result.FromDouble(1e30) }
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

    public void Add(string key, Result value)
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

    public Result this[string key]
    {
        get => _constants.ContainsKey(key) ? _constants[key] : _variables[key];
        set => Add(key, value);
    }

    public IEnumerable<KeyValuePair<string, Result>> Variables()
    {
        foreach (var item in _variables)
            yield return item;
    }

    public IEnumerable<KeyValuePair<string, Result>> Constants()
    {
        foreach (var item in _constants)
            yield return item;
    }

    public IEnumerator<KeyValuePair<string, Result>> GetEnumerator()
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
