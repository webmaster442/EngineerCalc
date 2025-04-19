
namespace DynamicEvaluator.Expressions.Specific;

internal sealed class VariableExpression : IExpression
{
    public VariableExpression(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier { get; }

    public IExpression Differentiate(string byVariable)
    {
        if (byVariable == Identifier)
        {
            // f(x) = x
            // d( f(x) ) = 1 * d( x )
            // d( x ) = 1
            // f'(x) = 1
            return new ConstantExpression(1L);
        }
        // f(x) = c
        // d( f(x) ) = c * d( c )
        // d( c ) = 0
        // f'(x) = 0
        return new ConstantExpression(0L);
    }

    public dynamic Evaluate(VariablesAndConstantsCollection variables) 
        => variables[Identifier];

    public IExpression Simplify()
        => new VariableExpression(Identifier);

    public string ToLatex()
        => $"{{ {Identifier} }}";

    public override string ToString()
        => Identifier;
}
