using System.Globalization;
using System.Text;

namespace DynamicEvaluator.Expressions;

internal static class Tokenizer
{
    private static bool IsAdditionalyAllowedInNumber(char c)
        => IsFloatCharacter(c) || c == '_';

    private static bool IsFloatCharacter(char c)
    {
        return c == '.'
            || c == '-'
            || c == '+'
            || c == 'E'
            || c == 'e';
    }

    private static Token HandleNumber(string input, int start, out int newIndex)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        bool containsAtLeastOneDigit = false;
        bool isInScientificMode = false;
        bool sciencemodePrefixed = false;
        bool dotFound = false;
        while (index < input.Length)
        {
            if (char.IsDigit(input[index]))
            {
                sb.Append(input[index]);
                ++index;
                containsAtLeastOneDigit = true;
            }
            else if (containsAtLeastOneDigit && IsAdditionalyAllowedInNumber(input[index]))
            {
                if (input[index] == '.' && !dotFound)
                {
                    dotFound = true;
                    sb.Append(input[index]);
                    ++index;
                }
                else if ((input[index] == '+' || input[index] == '-') && isInScientificMode)
                {
                    if (!sciencemodePrefixed)
                    {
                        sciencemodePrefixed = true;
                        sb.Append(input[index]);
                        ++index;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (input[index] == 'e' || input[index] == 'E' && !isInScientificMode)
                {
                    isInScientificMode = true;
                    sb.Append(input[index]);
                    ++index;
                }
                else if (input[index] == '_')
                {
                    sb.Append(input[index]);
                    ++index;
                }
                else
                {
                    break;
                }
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
    private static IEnumerable<string> SplitBySpaceWhenNotInQuotes(string input)
    {
        StringBuilder sb = new();
        bool inSingleQuote = false;
        bool inDoubleQuote = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '\'' && !inDoubleQuote)
            {
                inSingleQuote = !inSingleQuote;
                sb.Append(c);
            }
            else if (c == '"' && !inSingleQuote)
            {
                inDoubleQuote = !inDoubleQuote;
                sb.Append(c);
            }
            else if (char.IsWhiteSpace(c) && !inSingleQuote && !inDoubleQuote)
            {
                if (sb.Length > 0)
                {
                    var text = sb.ToString();
                    sb.Clear();
                    yield return text;
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }

    public static TokenCollection TokenizeRpn(string input, Predicate<string> isFunctionCheck)
    {
        IEnumerable<string> items = SplitBySpaceWhenNotInQuotes(input);
        TokenCollection tokens = new();
        foreach (var item in items)
        {
            if (long.TryParse(item, out _))
            {
                tokens.Add(new Token(item, TokenType.Constant, typeof(long)));
            }
            else if (double.TryParse(item, CultureInfo.InvariantCulture, out _))
            {
                tokens.Add(new Token(item, TokenType.Constant, typeof(double)));
            }
            else if ((item.StartsWith('\'') && item.EndsWith('\''))
                || (item.StartsWith('"') && item.EndsWith('"')))
            {
                tokens.Add(new Token(item[1..^1], TokenType.Constant, typeof(string)));
            }
            else if (item == "true" || item == "false")
            {
                tokens.Add(new Token(item, TokenType.Constant, typeof(bool)));
            }
            else if (item.Length == 1)
            {
                tokens.Add(HandleSingleCharOperator(item[0], -1));
            }
            else if (item.Length == 2)
            {
                tokens.Add(HandleOperator(item[0], item[1], -1, out _));
            }
            else if (IsIdentifier(item[0]) && item.All(IsIdentifier))
            {
                tokens.Add(new Token(item, TokenType.Variable));
            }
            else if (isFunctionCheck(item))
            {
                tokens.Add(new Token(item, TokenType.Function));
            }
        }
        return tokens;
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

                char current = input[index];
                char next = index +1 < input.Length ? input[index + 1] : '\0';
                Token @operator = HandleOperator(current, next, index, out newIndex);
                tokens.Add(@operator);
                index = newIndex;
            }
        }
        tokens.Add(new Token(string.Empty, TokenType.Eof));
        return tokens;
    }

    private static Token HandleOperator(char current, char next, int index, out int newIndex)
    {
        string combined = $"{current}{next}";
        switch (combined)
        {
            case "==":
                newIndex = index += 2;
                return new Token(combined, TokenType.Equal);
            case "!=":
                newIndex = index += 2;
                return new Token(combined, TokenType.NotEqual);
            case "<=":
                newIndex = index += 2;
                return new Token(combined, TokenType.LessThanOrEqual);
            case ">=":
                newIndex = index + 2;
                return new Token(combined, TokenType.GreaterThanOrEqual);
            default:
                newIndex = index + 1;
                return HandleSingleCharOperator(current, index);
        }
    }

    private static Token HandleSingleCharOperator(char current, int position)
    {
        return current switch
        {
            '<' => new Token("<", TokenType.LessThan),
            '>' => new Token(">", TokenType.GreaterThan),
            '|' => new Token("|", TokenType.Or),
            '&' => new Token("&", TokenType.And),
            '!' => new Token("!", TokenType.Not),
            '+' => new Token("+", TokenType.Plus),
            '-' => new Token("-", TokenType.Minus),
            '*' => new Token("*", TokenType.Multiply),
            '/' => new Token("/", TokenType.Divide),
            '%' => new Token("%", TokenType.Modulo),
            '^' => new Token("^", TokenType.Exponent),
            '(' => new Token("(", TokenType.OpenParen),
            ')' => new Token(")", TokenType.CloseParen),
            ',' => new Token(",", TokenType.ArgumentDivider),
            '.' => new Token(".", TokenType.MemberAccess),
            '=' => new Token("=", TokenType.Assignment),
            '?' => new Token("?", TokenType.TennaryIf),
            ':' => new Token(":", TokenType.TennaryElse),
            _ => throw new InvalidOperationException($"Invalid operator `{current}` at position {position}"),
        };
    }
}
