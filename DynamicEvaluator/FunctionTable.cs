namespace DynamicEvaluator;

internal class FunctionTable
{
    private readonly Dictionary<string, int> _table;

    public FunctionTable()
    {
        _table = new Dictionary<string, int>();
    }

    public void FillFrom(Type type)
    {
        var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        foreach (var method in methods)
        {
            _table.Add(method.Name, method.GetParameters().Length);
        }
    }

    public int GetParameterCount(string name)
    {
        if (!_table.ContainsKey(name))
            return -1;

        return _table[name];
    }
}
