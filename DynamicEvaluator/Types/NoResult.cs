namespace DynamicEvaluator.Types;

internal sealed class NoResult : IEquatable<NoResult?>
{
    public override bool Equals(object? obj)
    {
        return Equals(obj as NoResult);
    }

    public bool Equals(NoResult? other)
        => other is not null;

    public override int GetHashCode()
        => HashCode.Combine(int.MinValue);

    public override string ToString()
        => string.Empty;

    public static bool operator ==(NoResult? left, NoResult? right)
        => EqualityComparer<NoResult>.Default.Equals(left, right);

    public static bool operator !=(NoResult? left, NoResult? right)
        => !(left == right);
}
