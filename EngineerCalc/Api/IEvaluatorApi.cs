//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator;

namespace EngineerCalc.Api;

internal interface IEvaluatorApi
{
    IExpression Parse(string expression);
    IExpression ParseRpn(string expression);
    VariablesAndConstantsCollection VariablesAndConstants { get; }
}
