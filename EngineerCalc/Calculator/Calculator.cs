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

    public Task<string> Process(string input, string session)
    {
        return Task.Run(() =>
        {
            if (_memoryCache.TryGetValue(session, out State? state))
            {
                _memoryCache.Remove(session);
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
                    var result = expression.Simplify().Evaluate(state!.Variables);
                    htmlBuilder.AddResult(input, result.Stringify(state.Culture));
                }
            }
            catch (Exception ex)
            {
                htmlBuilder
                    .Reset()
                    .Exception(input, ex);
            }

            _memoryCache.Set(session, state, TimeSpan.FromMinutes(15));
            return htmlBuilder.ToString();
        });
    }

    private void RunHashMarkCommand(HtmlBuilder htmlBuilder)
    {
        throw new NotImplementedException();
    }
}
