//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator;

public interface IExpression : IEquatable<IExpression>
{
    /// <summary>
    /// Evaluate an expression
    /// </summary>
    /// <returns>Result of expression</returns>
    dynamic Evaluate(VariablesAndConstantsCollection variables);

    /// <summary>
    /// Diferentiate an expression
    /// </summary>
    /// <param name="byVariable">>Diferentation variable</param>
    /// <returns>Diferentiated expression</returns>
    IExpression Differentiate(string byVariable);

    /// <summary>
    /// Simplifies an expresion
    /// </summary>
    /// <returns>Simplified expression</returns>
    IExpression Simplify();

    /// <summary>
    /// Converts expression to string
    /// </summary>
    /// <returns>string</returns>
    string ToString();

    /// <summary>
    /// Converts expression to LaTex
    /// </summary>
    /// <returns>LaTex string</returns>
    string ToLatex();
}
