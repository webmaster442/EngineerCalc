//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions;

internal sealed class TokenCollection
{
    private readonly List<Token> _tokens;
    private int _index;

    public TokenCollection()
    {
        _tokens = new List<Token>();
        CurrentToken = new Token(string.Empty, TokenType.None);
        _index = -1;
    }

    public int Count => _tokens.Count;

    public void Add(Token token)
        => _tokens.Add(token);

    public Token CurrentToken
    {
        get; private set;
    }

    public bool Next()
    {
        ++_index;

        if (CurrentToken.Type == TokenType.Eof)
            throw new InvalidOperationException("Out of tokens");

        CurrentToken = _tokens[_index];

        return CurrentToken.Type != TokenType.Eof;
    }

    public void Eat(TokenType type)
    {
        if (CurrentToken.Type != type)
            throw new InvalidOperationException($"Expected a {type} token, got: {CurrentToken.Type}");

        Next();
    }

    public bool Check(TokenSet tokens)
        => tokens.Contains(CurrentToken.Type);
}
