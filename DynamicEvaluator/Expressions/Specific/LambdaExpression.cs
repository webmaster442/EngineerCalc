namespace DynamicEvaluator.Expressions.Specific;

internal sealed class LambdaExpression1 : UnaryExpression
{
    private readonly Func<dynamic, dynamic> _function;
    private readonly string _name;

    public LambdaExpression1(IExpression child, Func<dynamic, dynamic> function, string name) : base(child)
    {
        _function = function;
        _name = name;
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException($"Can't differentiate an expression with the function {_name}");

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new LambdaExpression1(newChild, _function, _name);
    }

    protected override dynamic Evalulate(dynamic value)
        => _function(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {_name}({Child}) }}"
            : $"{_name}({Child})";
    }
}
