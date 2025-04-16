using System.Globalization;
using System.Resources;
using System.Text;

using DynamicEvaluator.Expressions;
using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public sealed class ExpressionFactory
{
    private readonly TokenSet FirstMultExp;
    private readonly TokenSet FirstExpExp;
    private readonly TokenSet FirstUnaryExp;
    private readonly TokenSet FirstFactorPrefix;
    private readonly TokenSet FirstFactor;
    private readonly FunctionTable _functionTable;

    public ExpressionFactory()
    {
        var firstFunction = new TokenSet(TokenType.Function);
        FirstFactor = firstFunction + new TokenSet(TokenType.Variable, TokenType.OpenParen);
        FirstFactorPrefix = FirstFactor + TokenType.Constant;
        FirstUnaryExp = FirstFactorPrefix + TokenType.Minus + TokenType.Not;
        FirstExpExp = new TokenSet(FirstUnaryExp);
        FirstMultExp = new TokenSet(FirstUnaryExp);
        _functionTable = new FunctionTable();
        _functionTable.FillFrom(typeof(Functions));
    }

    public IExpression Create(string input)
    {
        TokenCollection tokens = Tokenizer.Tokenize(input, _functionTable.GetParameterCount);

        if (!tokens.Next())
            throw new InvalidOperationException("Can't create an expression from an empty input");

        var expression = ParseAddExpression(tokens);
        var leftover = new StringBuilder();
        while (tokens.CurrentToken.Type != TokenType.Eof)
        {
            leftover.Append(tokens.CurrentToken.Value);
            tokens.Next();
        }
        if (leftover.Length > 0)
            throw new InvalidOperationException($"Couldn't parse part of expression: {leftover}");

        return expression;
    }

    private IExpression ParseAddExpression(TokenCollection tokens)
    {
        if (tokens.Check(FirstMultExp))
        {
            var exp = ParseMultExpression(tokens);

            while (tokens.Check(new TokenSet(TokenType.Plus, TokenType.Minus, TokenType.Or)))
            {
                var opType = tokens.CurrentToken.Type;
                tokens.Eat(opType);
                if (!tokens.Check(FirstMultExp))
                {
                    throw new InvalidOperationException("Expected expression");
                }

                var right = ParseMultExpression(tokens);

                switch (opType)
                {
                    case TokenType.Plus:
                        exp = new AddExpression(exp, right);
                        break;

                    case TokenType.Or:
                        exp = new OrExpression(exp, right);
                        break;

                    case TokenType.Minus:
                        exp = new SubtractExpression(exp, right);
                        break;

                    default:
                        throw new InvalidOperationException($"Expcted as plus, minus, or. Got: {opType}");
                }
            }

            return exp;
        }
        throw new InvalidOperationException("Invalid expression");
    }

    private IExpression ParseMultExpression(TokenCollection tokens)
    {
        if (tokens.Check(FirstExpExp))
        {
            var exp = ParseExpExpression(tokens);

            while (tokens.Check(new TokenSet(TokenType.Multiply, TokenType.Divide, TokenType.And)))
            {
                var opType = tokens.CurrentToken.Type;
                tokens.Eat(opType);
                if (!tokens.Check(FirstExpExp))
                {
                    throw new InvalidOperationException("Expected expression after * or /");
                }
                var right = ParseExpExpression(tokens);

                switch (opType)
                {
                    case TokenType.Multiply:
                        exp = new MultiplyExpression(exp, right);
                        break;

                    case TokenType.Divide:
                        exp = new DivideExpression(exp, right);
                        break;

                    case TokenType.And:
                        exp = new AndExpression(exp, right);
                        break;

                    default:
                        throw new InvalidOperationException($"Expected * or /, got: {opType}");
                }
            }

            return exp;
        }
        throw new InvalidOperationException("Invalid expression");
    }

    private IExpression ParseExpExpression(TokenCollection tokens)
    {
        if (tokens.Check(FirstUnaryExp))
        {
            var exp = ParseUnaryExpression(tokens);

            if (tokens.Check(new TokenSet(TokenType.Exponent)))
            {
                var opType = tokens.CurrentToken.Type;
                tokens.Eat(opType);
                if (!tokens.Check(FirstUnaryExp))
                {
                    throw new InvalidOperationException("Expected expression after exponent");
                }
                var right = ParseUnaryExpression(tokens);

                switch (opType)
                {
                    case TokenType.Exponent:
                        exp = new ExponentExpression(exp, right);
                        break;

                    default:
                        throw new InvalidOperationException($"Expected ^ got: {opType}");
                }
            }

            return exp;
        }
        throw new InvalidOperationException("Invalid expression");
    }

    private IExpression ParseUnaryExpression(TokenCollection tokens)
    {
        var negate = false;
        var not = false;
        if (tokens.CurrentToken.Type == TokenType.Minus)
        {
            tokens.Eat(TokenType.Minus);
            negate = true;
        }
        if (tokens.CurrentToken.Type == TokenType.Not)
        {
            tokens.Eat(TokenType.Not);
            not = true;
        }

        if (tokens.Check(FirstFactorPrefix))
        {
            var exp = ParseFactorPrefix(tokens);

            if (negate)
            {
                return new NegateExpression(exp);
            }
            else if (not)
            {
                return new LogicNegateExpression(exp);
            }

            return exp;
        }
        throw new InvalidOperationException("Invalid expression");
    }

    private IExpression ParseFactorPrefix(TokenCollection tokens)
    {
        IExpression? exp = null;
        if (tokens.CurrentToken.Type == TokenType.Constant)
        {
            dynamic value = TypeFactory.CreateType(tokens.CurrentToken.Value, tokens.CurrentToken.Data);
            exp = new ConstantExpression(value);
            tokens.Eat(TokenType.Constant);
        }

        if (tokens.Check(FirstFactor))
        {
            if (exp == null)
            {
                return ParseFactor(tokens);
            }
            return new MultiplyExpression(exp, ParseFactor(tokens));
        }

        if (exp == null)
            throw new InvalidOperationException("Invalid expression");

        return exp;
    }

    private IExpression ParseFactor(TokenCollection tokens)
    {
        IExpression? exp = null;
        do
        {
            IExpression? right = null;
            switch (tokens.CurrentToken.Type)
            {
                case TokenType.Variable:
                    right = new VariableExpression(tokens.CurrentToken.Value);
                    tokens.Eat(TokenType.Variable);
                    break;

                case TokenType.Function:
                    right = ParseFunction(tokens);
                    break;

                case TokenType.OpenParen:
                    tokens.Eat(TokenType.OpenParen);
                    right = ParseAddExpression(tokens);
                    tokens.Eat(TokenType.CloseParen);
                    break;

                default:
                    throw new InvalidOperationException($"Unexpected token in factor: {tokens.CurrentToken.Type}");
            }

            exp = (exp == null) ? right : new MultiplyExpression(exp, right);
        } 
        while (tokens.Check(FirstFactor));

        return exp;
    }

    private IExpression ParseFunction(TokenCollection tokens)
    {
        var opType = tokens.CurrentToken.Type;
        var function = tokens.CurrentToken.Value.ToLower();
        int count = _functionTable.GetParameterCount(function);

        if (count == -1)
            throw new InvalidOperationException($"Unknown function: {function}");

        tokens.Eat(opType);
        tokens.Eat(TokenType.OpenParen);

        List<IExpression> parameters = new();
        for (int i = 0; i < count; i++)
        {
            parameters.Add(ParseAddExpression(tokens));

            if (i! < count - 1)
                tokens.Eat(TokenType.ArgumentDivider);
        }

        tokens.Eat(TokenType.CloseParen);

        return _functionTable.Create(function, parameters);
    }
}
