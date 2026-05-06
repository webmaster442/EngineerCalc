//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions;

internal abstract class BinaryExpression : IExpression
{
    protected BinaryExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public IExpression Left { get; }
    public IExpression Right { get; }

    public abstract IExpression Differentiate(string byVariable);

    public abstract IExpression Simplify();

    public Result Evaluate(VariablesAndConstantsCollection variables)
    {
        Result l = Left.Evaluate(variables);
        Result r = Right.Evaluate(variables);
        return Evaluate(l, r);
    }

    protected abstract Result Evaluate(Result value1, Result value2);

    protected abstract string Render(bool emitLatex);

    public string ToLatex()
        => Render(emitLatex: true);

    public override string ToString()
        => Render(emitLatex: false);

    public bool Equals(IExpression? other)
    {
        return other is BinaryExpression otherBinary
            && GetType() == otherBinary.GetType()
            && Left.Equals(otherBinary.Left)
            && Right.Equals(otherBinary.Right);
    }
}
