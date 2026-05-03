using System.Collections;

namespace DynamicEvaluator.TypeSystem.InternalTypes;

internal sealed class DoubleArray : IEnumerable<double>
{
    private readonly double[] _values;

    public DoubleArray(params double[] values)
    {
        _values = values;
    }

    public IEnumerator<double> GetEnumerator()
        => ((IEnumerable<double>)_values).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
