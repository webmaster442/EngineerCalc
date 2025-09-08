using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.Types;

namespace DynamicEvaluator.Expressions;

internal static class RpnExpressionFactory
{
    private static void CheckStackCount(Stack<IExpression> stack, int count)
    {
        if (stack.Count < count)
            throw new InvalidOperationException("Not enough expressions in stack for operator");
    }

    public static IExpression Create(TokenCollection tokens, FunctionProvider functionTable)
    {
        Stack<IExpression> exprStack = new Stack<IExpression>();

        while (tokens.Next())
        {
            if (tokens.CurrentToken.Type == TokenType.Eof)
                break;

            if (tokens.CurrentToken.Type == TokenType.ArgumentDivider)
                throw new InvalidOperationException("Unexpected argument divider in RPN expression");

            if (tokens.CurrentToken.Type == TokenType.OpenParen || tokens.CurrentToken.Type == TokenType.CloseParen)
                throw new InvalidOperationException("Unexpected parenthesis in RPN expression");

            switch (tokens.CurrentToken.Type)
            {
                case TokenType.Constant:
                    dynamic value = TypeFactory.CreateType(tokens.CurrentToken.Value, tokens.CurrentToken.TypeInfo);
                    exprStack.Push(new ConstantExpression(value));
                    break;
                case TokenType.Variable:
                    exprStack.Push(new VariableExpression(tokens.CurrentToken.Value));
                    break;
                case TokenType.Minus:
                case TokenType.Plus:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Modulo:
                case TokenType.Exponent:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.LessThan:
                case TokenType.LessThanOrEqual:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.Assignment:
                case TokenType.MemberAccess:
                    CheckStackCount(exprStack, 2);
                    var right = exprStack.Pop();
                    var left = exprStack.Pop();
                    exprStack.Push(CreateBinary(left, right, tokens.CurrentToken.Type));
                    break;
                case TokenType.Not:
                    CheckStackCount(exprStack, 1);
                    var expr = exprStack.Pop();
                    exprStack.Push(new LogicNegateExpression(expr));
                    break;
                case TokenType.Function:
                    List<IExpression> parameters = new();
                    for (int i=0; i <= exprStack.Count; i++)
                    {
                        parameters.Add(exprStack.Pop());
                    }
                    exprStack.Push(functionTable.Create(tokens.CurrentToken.Value, parameters));
                    break;
            }
        }
        return exprStack.Pop();
    }

    private static IExpression CreateBinary(IExpression left, IExpression right, TokenType type)
    {
        return type switch
        {
            TokenType.MemberAccess => new MemberAccessExpression(left, right),
            TokenType.Plus => new AddExpression(left, right),
            TokenType.Minus => new SubtractExpression(left, right),
            TokenType.Multiply => new MultiplyExpression(left, right),
            TokenType.Divide => new DivideExpression(left, right),
            TokenType.Modulo => new ModuloExpression(left, right),
            TokenType.Exponent => new ExponentExpression(left, right),
            TokenType.And => new AndExpression(left, right),
            TokenType.Or => new OrExpression(left, right),
            TokenType.LessThan => new LessThanExpression(left, right),
            TokenType.LessThanOrEqual => new LessThanOrEqualExpression(left, right),
            TokenType.GreaterThan => new GreaterThanExpression(left, right),
            TokenType.GreaterThanOrEqual => new GreaterThanOrEqualExpression(left, right),
            TokenType.Equal => new EqualExpression(left, right),
            TokenType.NotEqual => new NotEqualExpression(left, right),
            TokenType.Assignment => new AssignmentExpression(left, right),
            _ => throw new InvalidOperationException($"Unsupported binary operator: {type}"),
        };
    }
}
