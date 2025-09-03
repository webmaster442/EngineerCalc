using DynamicEvaluator;

namespace EngineerCalc.Api;

internal interface IEvaluatorApi
{
    IExpression Parse(string expression);
    VariablesAndConstantsCollection VariablesAndConstants { get; }
}
