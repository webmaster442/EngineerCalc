using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class CdCommand : Command<CdCommand.Arguments>
{
    private readonly State _state;

    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<path>")]
        public string Path { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                return ValidationResult.Error("Path cannot be empty.");
            }
            return base.Validate();
        }
    }

    public CdCommand(State state)
    {
        _state = state;
    }

    protected override int Execute(CommandContext context, Arguments settings, CancellationToken cancellationToken)
    {
        try
        {
            if (Path.IsPathFullyQualified(settings.Path))
            {
                Directory.SetCurrentDirectory(settings.Path);
                _state.CurrentDirectory = settings.Path;
            }
            else
            {
                var newPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), settings.Path));
                Directory.SetCurrentDirectory(newPath);
                _state.CurrentDirectory = newPath;
            }
            return ExitCodes.Success;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Couldn't change directory to: {settings.Path}", ex);
        }
    }
}
