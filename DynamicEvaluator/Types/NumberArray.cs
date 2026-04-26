using System.Collections;

namespace DynamicEvaluator.Types;

public sealed class NumberArray : IEnumerable<double>
{
    private readonly IReadOnlyList<double> _items;

    public NumberArray(IReadOnlyList<double> items)
    {
        _items = items;
    }

    public int Length
        => _items.Count;

    public IEnumerator<double> GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
        => $"[{string.Join(", ", _items)}]";
}
