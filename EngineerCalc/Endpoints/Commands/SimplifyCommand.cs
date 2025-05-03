using System.ComponentModel.DataAnnotations;

using DynamicEvaluator;

using Microsoft.AspNetCore.Mvc.Filters;

namespace EngineerCalc.Endpoints.Commands;

internal sealed class SimplifyCommand : CommandBase<SimplifyCommand.Settings>
{
    public class Settings
    {
        [Required]
        [Argument(0, "Expression to simplify")]
        public string Expression { get; set; } = string.Empty;
    }

    public override string Name => "simplify";

    private readonly ExpressionFactory _expressionFactory;

    public SimplifyCommand()
    {
        _expressionFactory = new();
    }

    protected override Task<Result> ExecuteInternal(State state, Settings parsed)
    {
        return Task.Run(() =>
        {
            HtmlBuilder htmlBuilder = new();
            try
            {
                IExpression result = _expressionFactory.Create(parsed.Expression);

                result = result.IsLogicExpression()
                    && result.TrySimplfyAsLogicExpression(out IExpression? logic)
                    ? logic
                    : result.Simplify();

                string latex = result.ToLatex();

                if (result.TryGetConstantValue(out object value))
                {
                    latex = value.Stringify(state.Culture);
                }
                htmlBuilder.AddResults(result.ToString(), latex);
                return Result.FromSuccess(htmlBuilder);
            }
            catch (Exception ex)
            {
                return Result.FromError(htmlBuilder.Reset().Exception(ex));
            }
        });
    }
}
