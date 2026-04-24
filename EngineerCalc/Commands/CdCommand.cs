using System.ComponentModel;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class CdCommand : FileSystemCommand<CdCommand.Arguments>
{
    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<path>")]
        [Description("The path to change the current directory to.")]
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

    public CdCommand(IFileSystem fileSystem, State state) : base(fileSystem, state)
    {
    }

    protected override async Task<int> ExecuteAsync(CommandContext context,
                                                    Arguments settings,
                                                    CancellationToken cancellationToken)
    {
        try
        {
            var pathToSet = GetFullPath(settings.Path);
            Directory.SetCurrentDirectory(pathToSet);
            _state.CurrentDirectory = pathToSet;

            return ExitCodes.Success;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Couldn't change directory to: {settings.Path}", ex);
        }
    }
}
