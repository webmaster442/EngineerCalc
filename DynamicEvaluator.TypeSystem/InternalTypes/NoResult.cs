namespace DynamicEvaluator.TypeSystem.InternalTypes;

internal readonly struct NoResult : IEquatable<NoResult>
{
    public override string ToString()
        => string.Empty;

    public bool Equals(NoResult other)
        => true;

    public override bool Equals(object? obj)
        => obj is NoResult;

    public override int GetHashCode()
        => 0;
}
