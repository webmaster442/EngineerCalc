using DynamicEvaluator;

namespace EngineerCalc.Api;

internal interface IEvaluatorApi
{
    IExpression Parse(string expression);
    IExpression ParseRpn(string expression);
    VariablesAndConstantsCollection VariablesAndConstants { get; }
}
