//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace DynamicEvaluator.Expressions;

internal readonly struct Token
{
    public Token(string value, TokenType type, Type? typeInfo = null)
    {
        Value = value;
        Type = type;
        TypeInfo = typeInfo;
    }
    public Type? TypeInfo { get; }
    public string Value { get; }
    public TokenType Type { get; }

    public override string ToString()
        => $"{Value} | {Type} | {TypeInfo}";
}
