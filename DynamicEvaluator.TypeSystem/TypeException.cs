namespace DynamicEvaluator.TypeSystem;

public sealed class TypeException : Exception
{
    public TypeException() : base()
    {
    }

    public TypeException(string? message) : base(message)
    {
    }

    public TypeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static TypeException Incompatible(TypeState left, TypeState right)
        => new TypeException($"Incompatible types: {left} and {right}.");

    public static TypeException IncompatibleOperator(TypeState left, TypeState right, string symbol)
        => new TypeException($"Incompatible types: {left} and {right} for operation: {symbol}");

    internal static Exception IncompatibleFunction(string functionName, params TypeState[] typeStates)
        => new TypeException($"Incompatible types for function '{functionName}': {string.Join(", ", typeStates)}.");

    public static TypeException ShouldNotHappen(TypeState typeState1, TypeState typeState2)
        => new TypeException($"Unexpected type states: {typeState1} and {typeState2}.");
}
