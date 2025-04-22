using System.ComponentModel.DataAnnotations;

using DynamicEvaluator;

namespace EngineerCalc.Endpoints.Commands;

internal sealed class SetCommand : CommandBase<SetCommand.Settings>
{
    public class Settings
    {
        [Required]
        [Argument(0, "Variable Name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Argument(1, "Expression to set to variable name")]
        public string Expression { get; set; } = string.Empty;
    }

    public override string Name => "set";

    protected override Task<Result> ExecuteInternal(State state, Settings settings)
    {
        var factory = new ExpressionFactory();
        var value = factory.Create(settings.Expression).Evaluate(state.Variables);
        state.Variables.Add(settings.Name, value);
        return Task.FromResult(Result.FromSuccess($"<p>{settings.Name} is now set to {value}</p>"));
    }
}
