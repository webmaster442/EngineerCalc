
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
        dynamic child = Child.Evaluate(variables);
        return Evaluate(child);
    }

    protected abstract dynamic Evaluate(dynamic value);

    protected abstract string Render(bool emitLatex);

    public string ToLatex()
        => Render(emitLatex: true);

    public override string ToString()
        => Render(emitLatex: false);
}