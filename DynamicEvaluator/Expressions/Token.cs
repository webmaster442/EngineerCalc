
namespace DynamicEvaluator.Expressions;

internal readonly struct Token
{
    public Token(string value, TokenType type, object? data = null)
    {
        Value = value;
        Type = type;
        Data = data;
    }
    public object? Data { get; }
    public string Value { get; }
    public TokenType Type { get; }
}
