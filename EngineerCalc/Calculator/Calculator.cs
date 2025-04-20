using DynamicEvaluator;

using Microsoft.Extensions.Caching.Memory;

namespace EngineerCalc.Calculator;

public sealed class Calculator
{
    private readonly MemoryCache _memoryCache;
    private readonly ExpressionFactory _expressionFactory;

    public Calculator()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions
        {
            TrackStatistics = true,
        });
        _expressionFactory = new ExpressionFactory();
    }

    public Task<(string response, bool ok)> Process(string input, string stateId)
    {
        return Task.Run(() =>
        {
            bool ok = true;

            if (_memoryCache.TryGetValue(stateId, out State? state))
            {
                _memoryCache.Remove(stateId);
            }
            else
            {
                state = new();
            }

            HtmlBuilder htmlBuilder = new();

            try
            {
                if (input.StartsWith('#'))
                {
                    RunHashMarkCommand(htmlBuilder);
                }
                else
                {
                    IExpression expression = _expressionFactory.Create(input);
                    object result = expression.Simplify().Evaluate(state!.Variables);
                    htmlBuilder.AddResult(result.Stringify(state.Culture));
                }
            }
            catch (Exception ex)
            {
                ok = false;
                htmlBuilder
                    .Reset()
                    .Exception(ex);
            }

            _memoryCache.Set(stateId, state, TimeSpan.FromMinutes(15));
            var str = htmlBuilder.ToString();

            return (str, ok);
        });
    }

    private void RunHashMarkCommand(HtmlBuilder htmlBuilder)
    {
        throw new NotImplementedException();
    }
}
