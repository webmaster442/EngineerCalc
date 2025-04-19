using System.Text;

namespace DynamicEvaluator.Expressions;

internal abstract class Tokenizer
{
    private static bool IsAdditionalyAllowedInNumber(char c )
        => IsFloatCharacter(c) || c == '_';

    private static bool IsFloatCharacter(char c)
    {
        return c == '.'
            || c == '-'
            || c == 'E'
            || c == 'e';
    }

    private static bool NextIsDigit(string input, int index)
    {
        char next = (index + 1) <= (input.Length - 1)
            ? input[index + 1]
            : ' ';
        return char.IsDigit(next);
    }

    private static Token HandleNumber(string input, int start, out int newIndex)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        bool containsAtLeastOneDigit = false;
        while (index < input.Length)
        {
            if (char.IsDigit(input[index]))
            {
                sb.Append(input[index]);
                ++index;
                containsAtLeastOneDigit = true;
            }
            else if (containsAtLeastOneDigit && IsAdditionalyAllowedInNumber(input[index]) && NextIsDigit(input, index))
            {
                sb.Append(input[index]);
                ++index;
            }
            else
            {
                break;
            }
        }
        newIndex = index;
        string tokenValue = sb.ToString();
        if (tokenValue.Any(IsFloatCharacter))
            return new Token(tokenValue, TokenType.Constant, typeof(double));
        else
            return new Token(tokenValue, TokenType.Constant, typeof(long));
    }

    private static Token HandleStringLiteral(string input, int start, char matcher, out int newIndex)
    {
        StringBuilder sb = new StringBuilder();
        int index = start + 1;
        while (index < input.Length)
        {
            if (input[index] == matcher)
            {
                index++;
                break;
            }
            else
            {
                sb.Append(input[index]);
                index++;
            }
        }
        newIndex = index;
        return new Token(sb.ToString(), TokenType.Constant, typeof(string));
    }

    private static bool IsIdentifier(char c)
        => c == '_' || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');

    private static Token HandleIdentifier(string input,
                                          int start,
                                          Predicate<string> isFunctionCheck,
                                          out int newIndex)
    {
        StringBuilder sb = new();
        int index = start;
        while (index < input.Length)
        {
            if (IsIdentifier(input[index]))
            {
                sb.Append(input[index]);
                ++index;
            }
            else
            {
                break;
            }
        }
        
        string identifier = sb.ToString();
        newIndex = index;
        switch(identifier)
        {
            case "true":
            case "false":
                {
                    return new Token(identifier, TokenType.Constant, typeof(bool));
                }
            default:
                {
                    bool isFunction = isFunctionCheck.Invoke(identifier);
                    return isFunction 
                        ? new Token(identifier, TokenType.Function) 
                        : new Token(identifier, TokenType.Variable);
                }
        }
    }

    public static TokenCollection Tokenize(string input, Predicate<string> isFunctionCheck)
    {
        TokenCollection tokens = new();
        int index = 0;
        int newIndex = 0;
        while (index < input.Length)
        {
            if (char.IsNumber(input[index]))
            {
                Token number = HandleNumber(input, index, out newIndex);
                tokens.Add(number);
                index = newIndex;
            }
            else if (input[index] == '"')
            {
                Token stringLiteral = HandleStringLiteral(input, index, '"', out newIndex);
                tokens.Add(stringLiteral);
                index = newIndex;
            }
            else if (input[index] == '\'')
            {
                Token stringLiteral = HandleStringLiteral(input, index, '\'', out newIndex);
                tokens.Add(stringLiteral);
                index = newIndex;
            }
            else if (IsIdentifier(input[index]))
            {
                Token identifier = HandleIdentifier(input, index, isFunctionCheck, out newIndex);
                tokens.Add(identifier);
                index = newIndex;
            }
            else
            {
                if (input[index] <= ' ')
                {
                    ++index;
                    continue;
                }
                Token op = HandleOperator(input[index], index);
                tokens.Add(op);
                ++index;
            }
        }
        tokens.Add(new Token(string.Empty, TokenType.Eof));
        return tokens;
    }

    private static Token HandleOperator(char c, int position)
    {
        return c switch
        {
            '|' => new Token("|", TokenType.Or),
            '&' => new Token("&", TokenType.And),
            '!' => new Token("!", TokenType.Not),
            '+' => new Token("+", TokenType.Plus),
            '-' => new Token("-", TokenType.Minus),
            '*' => new Token("*", TokenType.Multiply),
            '/' => new Token("/", TokenType.Divide),
            '^' => new Token("^", TokenType.Exponent),
            '(' => new Token("(", TokenType.OpenParen),
            ')' => new Token(")", TokenType.CloseParen),
            ',' => new Token(",", TokenType.ArgumentDivider),
            _ => throw new InvalidOperationException($"Invalid operator `{c}` at {position}"),
        };
    }
}
