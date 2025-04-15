
namespace DynamicEvaluator.Expressions;

internal abstract class UnaryExpression : IExpression
{
    public IExpression Child { get; }

    protected UnaryExpression(IExpression child)
    {
        Child = child;
    }

    public abstract IExpression Differentiate(string byVariable);

    public abstract IExpression Simplify();

    public dynamic Evaluate(IReadOnlyDictionary<string, dynamic> variables)
    {
        return Evaluate(Child.Evaluate(variables));
    }

    protected abstract dynamic Evalulate(dynamic value);

    protected abstract string Render(bool emitLatex);

    public string ToLatex()
        => Render(emitLatex: true);

    public override string ToString()
        => Render(emitLatex: false);
}