using System.Reflection;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class MemberAccessExpression : IExpression
{
    private readonly IExpression _expression;
    private readonly string _memberName;

    public MemberAccessExpression(IExpression left, IExpression right)
    {
        _expression = left;
        if (right is not VariableExpression variableExpression)
            throw new InvalidOperationException("Right expression must be an Identifier");

        _memberName = variableExpression.Identifier;
    }

    private MemberAccessExpression(IExpression expression, string memberName)
    {
        _expression = expression;
        _memberName = memberName;
    }

    private dynamic GetMember(dynamic obj)
    {
        Type type = obj.GetType();
        var property = type.GetProperty(_memberName, BindingFlags.Public | BindingFlags.Instance);
        if (property != null)
        {
            return property.GetValue(obj);
        }
        var field = type.GetField(_memberName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            return field.GetValue(obj);
        }
        throw new InvalidOperationException($"Member '{_memberName}' not found in type '{type.FullName}'.");
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Member access expressions cannot be differentiated directly. Consider differentiating the expression before accessing its members.");

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
    {
        return GetMember(_expression.Evaluate(variables));
    }

    public IExpression Simplify()
    {
        var simplified = _expression.Simplify();
        if (simplified is ConstantExpression constant)
        {
            return new ConstantExpression(GetMember(constant.Value));
        }
        return new MemberAccessExpression(simplified, _memberName);
    }

    public string ToLatex()
        => $"{{ {_expression.ToLatex()}.{_memberName} }}";

    public override string ToString()
        => $"{_expression.ToString()}.{_memberName}";
}
