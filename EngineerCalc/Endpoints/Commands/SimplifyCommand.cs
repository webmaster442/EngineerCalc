using System.ComponentModel.DataAnnotations;

using DynamicEvaluator;

namespace EngineerCalc.Endpoints.Commands;

internal sealed class SimplifyCommand : CommandBase<SimplifyCommand.Settings>
{
    public class Settings
    {
        [Required]
        [Argument(1, "Expression to simplify")]
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
                IExpression result = _expressionFactory.Create(parsed.Expression).Simplify();
                htmlBuilder.AddResults(result.ToString(), result.ToLatex());
                return Result.FromSuccess(htmlBuilder);
            }
            catch (Exception ex)
            {
                return Result.FromError(htmlBuilder.Reset().Exception(ex));
            }
        });
    }
}
