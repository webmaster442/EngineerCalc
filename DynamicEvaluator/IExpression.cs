namespace DynamicEvaluator;

public interface IExpression
{
    /// <summary>
    /// Evaluate an expression
    /// </summary>
    /// <returns>Result of expression</returns>
    dynamic Evaluate(Variables variables);

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
