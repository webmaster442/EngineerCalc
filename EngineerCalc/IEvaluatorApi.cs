using DynamicEvaluator;

namespace EngineerCalc;

internal interface IEvaluatorApi
{
    IExpression Parse(string expression);
    VariablesAndConstantsCollection VariablesAndConstants { get; }
}