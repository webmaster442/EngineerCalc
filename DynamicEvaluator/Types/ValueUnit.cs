using System.Numerics;

using UnitsNet;

namespace DynamicEvaluator.Types;

public sealed class ValueUnit :
    IAdditionOperators<ValueUnit, ValueUnit, ValueUnit>,
    ISubtractionOperators<ValueUnit, ValueUnit, ValueUnit>,
    IMultiplyOperators<ValueUnit, ValueUnit, ValueUnit>,
    IDivisionOperators<ValueUnit, ValueUnit, ValueUnit>,
    IAdditionOperators<ValueUnit, double, ValueUnit>,
    ISubtractionOperators<ValueUnit, double, ValueUnit>,
    IMultiplyOperators<ValueUnit, double, ValueUnit>,
    IDivisionOperators<ValueUnit, double, ValueUnit>,
    IEquatable<ValueUnit>
{
    private static readonly Dictionary<string, Enum> _lookupTable
        = Quantity.Infos
            .SelectMany(q => q.UnitInfos)
            .DistinctBy(q => q.Name)
            .ToDictionary(q => q.Name, q => q.Value, StringComparer.InvariantCultureIgnoreCase);

    private readonly IQuantity _quantity;

    internal ValueUnit(IQuantity quantity)
    {
        _quantity = quantity;
    }

    public ValueUnit(double value, string unit)
    {
        if (string.IsNullOrEmpty(unit))
        {
            _quantity = new Ratio(value, UnitsNet.Units.RatioUnit.DecimalFraction);
            return;
        }

        if (!_lookupTable.TryGetValue(unit, out Enum? unitEnum))
            throw new ArgumentException($"Unknown unit '{unit}'.", nameof(unit));

        _quantity = Quantity.From(value, unitEnum);
    }

    public override string ToString()
        => _quantity.ToString() ?? "";

    public bool Equals(ValueUnit? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        // Compare the underlying quantities
        return _quantity.Equals(other._quantity);
    }

    public override bool Equals(object? obj)
        => Equals(obj as ValueUnit);

    public override int GetHashCode()
        => _quantity.GetHashCode();

    public static ValueUnit operator -(ValueUnit left, ValueUnit right)
    {
        dynamic l = left._quantity;
        dynamic r = right._quantity;
        return new ValueUnit((IQuantity)(l - r));
    }

    public static ValueUnit operator +(ValueUnit left, ValueUnit right)
    {
        dynamic l = left._quantity;
        dynamic r = right._quantity;
        return new ValueUnit((IQuantity)(l + r));
    }

    public static ValueUnit operator *(ValueUnit left, ValueUnit right)
    {
        dynamic l = left._quantity;
        dynamic r = right._quantity;
        return new ValueUnit((IQuantity)(l * r));
    }

    public static ValueUnit operator /(ValueUnit left, ValueUnit right)
    {
        dynamic l = left._quantity;
        dynamic r = right._quantity;

        dynamic result = l / r;

        if (result is IQuantity quantity)
            return new ValueUnit(quantity);

        if (result is double d)
            return new ValueUnit(new Ratio(d, UnitsNet.Units.RatioUnit.DecimalFraction));

        throw new InvalidOperationException();
    }

    public static ValueUnit operator +(ValueUnit left, double right)
    {
        double value = (double)left._quantity.Value;
        return new ValueUnit(Quantity.From((value + right), left._quantity.Unit));
    }

    public static ValueUnit operator -(ValueUnit left, double right)
    {
        double value = (double)left._quantity.Value;
        return new ValueUnit(Quantity.From((value - right), left._quantity.Unit));
    }

    public static ValueUnit operator *(ValueUnit left, double right)
    {
        double value = (double)left._quantity.Value;
        return new ValueUnit(Quantity.From((value * right), left._quantity.Unit));
    }

    public static ValueUnit operator /(ValueUnit left, double right)
    {
        double value = (double)left._quantity.Value;
        return new ValueUnit(Quantity.From((value / right), left._quantity.Unit));
    }

    public static implicit operator double(ValueUnit valueUnit)
        => (double)valueUnit._quantity.Value;

    public ValueUnit ToOptimal()
    {
        var quantityInfo = Quantity.Infos.FirstOrDefault(q => q.Name == _quantity.QuantityInfo.Name);
        if (quantityInfo == null)
            return this; // No info found, return as-is

        // Find the unit where converted value's abs is closest to 1 but between 1 and 1000 if possible
        var bestUnit = quantityInfo.UnitInfos
            .Select(unitInfo =>
            {
                try
                {
                    double convertedValue = (double)_quantity.ToUnit(unitInfo.Value).Value;
                    return new
                    {
                        Unit = unitInfo,
                        AbsValue = Math.Abs(convertedValue),
                        ConvertedValue = convertedValue
                    };
                }
                catch
                {
                    return null; // ignore units where conversion fails
                }
            })
            .Where(x => x != null)
            // Prefer abs values between 1 and 1000
            .OrderBy(x => x!.AbsValue < 1 ? 1 - x.AbsValue : (x.AbsValue > 1000 ? x.AbsValue - 1000 : 0))
            .ThenBy(x => x!.AbsValue) // break ties with smaller absolute value
            .FirstOrDefault();

        if (bestUnit == null)
            return this; // No better unit found, return as-is

        return new ValueUnit(bestUnit.ConvertedValue, bestUnit.Unit.Name);
    }
}
