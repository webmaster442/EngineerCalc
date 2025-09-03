using DynamicEvaluator;

namespace EngineerCalc.Api;

internal class EvaluatorApi : IEvaluatorApi
{
    private readonly ExpressionFactory _expressionFactory;

    public EvaluatorApi(VariablesAndConstantsCollection variablesAndConstants, ExpressionFactory expressionFactory)
    {
        VariablesAndConstants = variablesAndConstants;
        _expressionFactory = expressionFactory;
    }

    public IExpression Parse(string expression)
    {
        return _expressionFactory.Create(expression);
    }

    public VariablesAndConstantsCollection VariablesAndConstants { get; }
}