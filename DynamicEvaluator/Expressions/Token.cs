//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions;

internal readonly struct Token
{
    public Token(string value, TokenType type, TypeState? typeState = null)
    {
        Value = value;
        Type = type;
        TypeState = typeState;
    }
    public TypeState? TypeState { get; }
    public string Value { get; }
    public TokenType Type { get; }

    public override string ToString()
        => $"{Value} | {Type} | {TypeState}";
}
