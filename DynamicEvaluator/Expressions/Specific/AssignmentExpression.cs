namespace DynamicEvaluator.Expressions.Specific;

internal sealed class AssignmentExpression : IExpression
{
    private readonly VariableExpression _variable;
    private readonly IExpression _expression;

    public AssignmentExpression(IExpression variable, IExpression expression)
    {
        if (variable is not VariableExpression varExpr)
            throw new InvalidOperationException("Left side of assignment must be a variable.");

        _variable = varExpr;
        _expression = expression;
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Assignment expressions cannot be differentiated.");

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
    {
        variables[_variable.Identifier] = _expression.Simplify().Evaluate(variables);
        return new object();
    }

    public IExpression Simplify()
    {
        var newExpression = _expression.Simplify();
        return new AssignmentExpression(_variable, newExpression);
    }

    public string ToLatex()
        => $"{_variable.ToLatex()} = {_expression.ToLatex()}";

    public override string ToString()
        => $"{_variable} = {_expression}";
}
