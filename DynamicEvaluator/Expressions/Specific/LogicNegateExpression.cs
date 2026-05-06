//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class LogicNegateExpression : UnaryExpression
{
    public LogicNegateExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the ~ operator");

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Result.FromBoolean(!childConst.Value.CastToBoolean()));
        }
        if (newChild is LogicNegateExpression negated)
        {
            // !!x -> x
            return negated.Child;
        }

        // !(x & y) -> x | y
        if (newChild is AndExpression and)
        {
            return new OrExpression(and.Left, and.Right);
        }

        // !(x | y) -> x & y
        if (newChild is OrExpression or)
        {
            return new AndExpression(or.Left, or.Right);
        }

        return new LogicNegateExpression(newChild);
    }

    protected override Result Evaluate(Result value)
        => Result.FromBoolean(!value.CastToBoolean());

    protected override string Render(bool emitLatex)
    {
        return emitLatex
             ? $"{{ \\neg {Child.ToLatex()} }}"
             : $"(!{Child})";
    }
}
