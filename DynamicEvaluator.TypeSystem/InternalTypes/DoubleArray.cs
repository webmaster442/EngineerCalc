//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;

namespace DynamicEvaluator.TypeSystem.InternalTypes;

internal sealed class DoubleArray : IReadOnlyList<double>
{
    private readonly IReadOnlyList<double> _values;

    public DoubleArray(IReadOnlyList<double> values)
    {
        _values = values;
    }

    public double this[int index]
        => _values[index];

    public int Count
        => _values.Count;

    public IEnumerator<double> GetEnumerator()
        => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _values.GetEnumerator();
}
