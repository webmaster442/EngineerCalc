//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Numerics;

using DynamicEvaluator.TypeSystem;
using DynamicEvaluator.TypeSystem.InternalTypes;

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

    private Result GetMember(Result obj)
    {
        switch (obj.TypeState)
        {
            case TypeState.Fraction:
                return GetFractionMember(obj.CastToFraction());
            case TypeState.Complex:
                return GetComplexTypeMember(obj.CastToComplex());
            case TypeState.Array:
            case TypeState.String:
            default:
                throw new InvalidOperationException($"There are no members of type {obj.TypeState} that can be accessed");
        }
    }

    private bool IsMemberName(string name)
        => string.Equals(name, _memberName, StringComparison.InvariantCultureIgnoreCase);

    private Result GetFractionMember(Fraction fraction)
    {
        if (IsMemberName(nameof(Fraction.Numerator)))
        {
            return Result.FromBigInteger(fraction.Numerator);
        }
        if (IsMemberName(nameof(Fraction.Denominator)))
        {
            return Result.FromBigInteger(fraction.Denominator);
        }
        throw new InvalidOperationException($"{_memberName} is not found in type {TypeState.Fraction}");
    }

    private Result GetComplexTypeMember(Complex complex)
    {
        if (IsMemberName(nameof(Complex.Real)))
        {
            return Result.FromDouble(complex.Real);
        }
        if (IsMemberName(nameof(Complex.Imaginary)))
        {
            return Result.FromDouble(complex.Imaginary);
        }
        if (IsMemberName(nameof(Complex.Magnitude)))
        {
            return Result.FromDouble(complex.Magnitude);
        }
        if (IsMemberName(nameof(Complex.Phase)))
        {
            return Result.FromDouble(complex.Phase);
        }
        throw new InvalidOperationException($"{_memberName} is not found in type {TypeState.Complex}");
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Member access expressions cannot be differentiated directly. Consider differentiating the expression before accessing its members.");

    public Result Evaluate(VariablesAndConstantsCollection variables)
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

    public bool Equals(IExpression? other)
    {
        return other is MemberAccessExpression otherMember
            && _memberName == otherMember._memberName
            && _expression.Equals(otherMember._expression);
    }
}
