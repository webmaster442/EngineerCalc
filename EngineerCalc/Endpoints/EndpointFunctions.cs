
using DynamicEvaluator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EngineerCalc.Endpoints;

internal class EndpointFunctions
{
    private readonly ExpressionFactory _expressionFactory;

    public EndpointFunctions()
    {
        _expressionFactory = new ExpressionFactory();
    }

    public Task<(string response, bool ok)> Evaluate(State state, string expression)
    {
        return Task.Run(() =>
        {
            HtmlBuilder htmlBuilder = new();
            bool ok = true;
            try
            {
                IExpression parsed = _expressionFactory.Create(expression);
                object result = parsed.Simplify().Evaluate(state!.Variables);
                htmlBuilder.AddResult(result.Stringify(state.Culture));
            }
            catch (Exception ex)
            {
                ok = false;
                htmlBuilder
                    .Reset()
                    .Exception(ex);
            }
            return (htmlBuilder.ToString(), ok);
        });
    }
}
