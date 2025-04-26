using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

using DynamicEvaluator.InternalFcuntions;

namespace DynamicEvaluator.Types;

/// <summary>
/// Represents a fractional number.
/// </summary>
public readonly struct Fraction :
    IEquatable<Fraction>,
    IComparable<Fraction>,
    IParsable<Fraction>,
    IEqualityOperators<Fraction, Fraction, bool>,
    IComparisonOperators<Fraction, Fraction, bool>,
    IUnaryPlusOperators<Fraction, Fraction>,
    IUnaryNegationOperators<Fraction, Fraction>,
    IIncrementOperators<Fraction>,
    IDecrementOperators<Fraction>,
    IMinMaxValue<Fraction>,
    IAdditiveIdentity<Fraction, Fraction>,
    IMultiplicativeIdentity<Fraction, Fraction>,
    IAdditionOperators<Fraction, Fraction, Fraction>,
    ISubtractionOperators<Fraction, Fraction, Fraction>,
    IDivisionOperators<Fraction, Fraction, Fraction>,
    IMultiplyOperators<Fraction, Fraction, Fraction>,
    IModulusOperators<Fraction, Fraction, Fraction>,
    IFormattable
{
    /// <summary>
    /// Creates a new instance of Fraction
    /// </summary>
    public Fraction() : this(0, 1) { }

    /// <summary>
    /// Creates a new instance of Fraction with the specified numerator and denominator.
    /// </summary>
    /// <param name="numerator">numerator</param>
    /// <param name="denominator">denominator</param>
    /// <exception cref="DivideByZeroException">when denominator is 0</exception>
    public Fraction(long numerator, long denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException();
        }

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        long gcd = Integers.GreatestCommonDivisor(numerator, denominator);
        Numerator = numerator / gcd;
        Denominator = denominator / gcd;
    }

    /// <inheritdoc/>
    public static Fraction AdditiveIdentity => new(0, 1);

    /// <inheritdoc/>
    public static Fraction MaxValue => new(long.MaxValue, 1);

    /// <inheritdoc/>
    public static Fraction MinValue => new(long.MinValue, 1);

    /// <inheritdoc/>
    public static Fraction MultiplicativeIdentity => new(1, 1);

    /// <summary>
    /// Gets the denominator of the fraction.
    /// </summary>
    public long Denominator { get; }


    /// <summary>
    /// Gets the numerator of the fraction.
    /// </summary>
    public long Numerator { get; }


    /// <summary>
    /// Converts the specified fraction to a double-precision floating-point number.
    /// </summary>
    /// <param name="fraction">Fraction to convert</param>
    public static implicit operator double(Fraction fraction)
    {
        return (double)fraction.Numerator / fraction.Denominator;
    }

    /// <summary>
    /// Converts the specified fraction to a single-precision floating-point number.
    /// </summary>
    /// <param name="fraction">Fraction to convert</param>
    public static implicit operator float(Fraction fraction)
    {
        return (float)fraction.Numerator / fraction.Denominator;
    }

    /// <summary>
    /// Creates a new instance of fraction from the specified number.
    /// </summary>
    /// <param name="number">An integer number</param>
    public static implicit operator Fraction(long number)
    {
        return new Fraction(number, 1);
    }
    /// <inheritdoc/>
    public static Fraction operator -(Fraction left, Fraction right)
    {
        long lcm = Integers.Lcm(left.Denominator, right.Denominator);
        long factorLeft = lcm / left.Denominator;
        long factorRigt = lcm / right.Denominator;
        long numerator = left.Numerator * factorLeft - right.Numerator * factorRigt;
        return new Fraction(numerator, lcm);
    }

    /// <inheritdoc/>
    public static Fraction operator -(Fraction value)
    {
        return new Fraction(-value.Numerator, value.Denominator);
    }

    /// <inheritdoc/>
    public static Fraction operator --(Fraction value)
    {
        return value - 1;
    }

    /// <inheritdoc/>
    public static bool operator !=(Fraction left, Fraction right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public static Fraction operator %(Fraction left, Fraction right)
    {
        long lcm = Integers.Lcm(left.Denominator, right.Denominator);
        long numerator1 = lcm / left.Denominator * left.Numerator;
        long numerator2 = lcm / right.Denominator * right.Numerator;
        return new Fraction(numerator1 % numerator2, lcm);
    }

    /// <inheritdoc/>
    public static Fraction operator *(Fraction left, Fraction right)
    {
        long numerator = left.Numerator * right.Numerator;
        long denominator = left.Denominator * right.Denominator;
        return new Fraction(numerator, denominator);
    }

    /// <inheritdoc/>
    public static Fraction operator /(Fraction left, Fraction right)
    {
        long numerator = left.Numerator * right.Denominator;
        long denominator = left.Denominator * right.Numerator;
        return new Fraction(numerator, denominator);
    }

    /// <inheritdoc/>
    public static Fraction operator +(Fraction left, Fraction right)
    {
        long lcm = Integers.Lcm(left.Denominator, right.Denominator);
        long factorLeft = lcm / left.Denominator;
        long factorRigt = lcm / right.Denominator;
        long numerator = left.Numerator * factorLeft + right.Numerator * factorRigt;
        return new Fraction(numerator, lcm);
    }

    /// <inheritdoc/>
    public static Fraction operator +(Fraction value)
    {
        return new Fraction(+value.Numerator, value.Denominator);
    }

    /// <inheritdoc/>
    public static Fraction operator ++(Fraction value)
    {
        return value + 1;
    }

    /// <inheritdoc/>
    public static bool operator <(Fraction left, Fraction right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(Fraction left, Fraction right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator ==(Fraction left, Fraction right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator >(Fraction left, Fraction right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(Fraction left, Fraction right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <inheritdoc/>
    public static Fraction Parse(string s, IFormatProvider? provider)
    {
        string[] parts = s.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
            return new Fraction(long.Parse(parts[0], provider), 1);
        else if (parts.Length == 2)
            return new Fraction(long.Parse(parts[0], provider), long.Parse(parts[1], provider));
        else
            throw new FormatException();
    }

    /// <inheritdoc/>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
    {
        try
        {
            result = Parse(s ?? string.Empty, provider);
            return true;
        }
        catch (Exception)
        {
            result = default;
            return false;
        }
    }

    /// <inheritdoc/>
    public readonly int CompareTo(Fraction other)
    {
        long n1 = Numerator * other.Denominator;
        long n2 = other.Numerator * Denominator;
        return n1.CompareTo(n2);
    }

    /// <inheritdoc/>
    public override readonly bool Equals(object? obj)
    {
        return obj is Fraction fraction && Equals(fraction);
    }

    /// <inheritdoc/>
    public readonly bool Equals(Fraction other)
    {
        return Numerator == other.Numerator &&
               Denominator == other.Denominator;
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode()
        => HashCode.Combine(Numerator, Denominator);

    /// <inheritdoc/>
    public override readonly string ToString()
        => ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <param name="culture">Culture to use</param>
    /// <returns>a string representation of the object</returns>
    public readonly string ToString(CultureInfo culture)
        => ToString("N0", culture);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string numerator = Numerator.ToString(format, formatProvider);

        if (Denominator == 1)
            return numerator;

        string denominator = Denominator.ToString(format, formatProvider);

        return $"{numerator} / {denominator}";
    }

    public static Fraction Abs(Fraction f)
    {
        long newNumerator = f.Numerator < 0 ? -1L * f.Numerator : f.Numerator;
        return new Fraction(newNumerator, f.Denominator);
    }
}