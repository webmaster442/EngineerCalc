namespace DynamicEvaluator.Expressions.Specific;

internal abstract class RewritableFunctionExpression : IExpression
{
    private readonly IExpression _rewritten;

    public RewritableFunctionExpression(IExpression original)
    {
        _rewritten = RewriteTo(original);
    }

    protected abstract IExpression RewriteTo(IExpression original);

    public IExpression Differentiate(string byVariable)
        => _rewritten.Differentiate(byVariable);

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
        => _rewritten.Evaluate(variables);

    public IExpression Simplify()
        => _rewritten.Simplify();

    public string ToLatex()
        => _rewritten.ToLatex();

    public override string ToString()
        => _rewritten.ToString();
}
