
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

    public dynamic Evaluate(IReadOnlyDictionary<string, dynamic> variables)
    {
        dynamic l = Left.Evaluate(variables);
        dynamic r = Right.Evaluate(variables);
        return Evaluate(l, r);
    }

    protected abstract dynamic Evaluate(dynamic value1, dynamic value2);

    protected abstract string Render(bool emitLatex);

    public string ToLatex()
        => Render(emitLatex: true);

    public override string ToString()
        => Render(emitLatex: false);
}
