using System.ComponentModel.DataAnnotations;

namespace EngineerCalc.Endpoints.Commands;

internal sealed class UnSetCommand : CommandBase<UnSetCommand.Settings>
{
    public class Settings
    {
        [Required]
        [Argument(0, "Variable Name")]
        public string Name { get; set; } = string.Empty;
    }

    public override string Name => "unset";

    protected override Task<Result> ExecuteInternal(State state, Settings settings)
    {
        if (state.Variables.Remove(settings.Name))
        {
            return Task.FromResult(Result.SuccessToHtml(string.Empty));
        }
        return Task.FromResult(Result.ErrorToHtml($"{settings.Name} was not set"));
    }
}