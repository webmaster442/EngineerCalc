//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
        => _expressionFactory.Create(expression);

    public IExpression ParseRpn(string expression)
        => _expressionFactory.CreateFromRpn(expression);

    public VariablesAndConstantsCollection VariablesAndConstants { get; }
}
