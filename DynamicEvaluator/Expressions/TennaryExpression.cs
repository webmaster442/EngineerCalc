//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions;

internal sealed class TennaryExpression : IExpression
{
    public TennaryExpression(IExpression condition, IExpression ifTrue, IExpression ifFalse)
    {
        Condition = condition;
        IfTrue = ifTrue;
        IfFalse = ifFalse;
    }

    public IExpression Condition { get; }
    public IExpression IfTrue { get; }
    public IExpression IfFalse { get; }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Cannot differentiate a tennary expression");

    public bool Equals(IExpression? other)
    {
        return other is TennaryExpression otherTennary
            && Condition.Equals(otherTennary.Condition)
            && IfTrue.Equals(otherTennary.IfTrue)
            && IfFalse.Equals(otherTennary.IfFalse);
    }

    public Result Evaluate(VariablesAndConstantsCollection variables)
    {
        Result cond = Condition.Evaluate(variables);
        return cond.CastToBoolean() ? IfTrue.Evaluate(variables) : IfFalse.Evaluate(variables);
    }

    public IExpression Simplify()
    {
        var newCondition = Condition.Simplify();
        var newIfTrue = IfTrue.Simplify();
        var newIfFalse = IfFalse.Simplify();
        if (newCondition is ConstantExpression condConst)
        {
            // condition is constant
            return condConst.Value.CastToBoolean() ? newIfTrue : newIfFalse;
        }
        // no simplification
        return new TennaryExpression(newCondition, newIfTrue, newIfFalse);
    }

    public string ToLatex()
        => $"\\begin{{cases}} {IfTrue.ToLatex()}, & {Condition.ToLatex()} \\\\ {IfFalse.ToLatex()}, & \\text{{otherwise}} \\end{{cases}}";

    public override string ToString()
        => $"({Condition} ? {IfTrue} : {IfFalse})";
}
