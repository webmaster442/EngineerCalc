using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ScriptCommand : FileSystemCommand<ScriptCommand.Arguments>
{
    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<scriptPath>")]
        public string ScriptPath { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(ScriptPath))
            {
                return ValidationResult.Error("Script path cannot be empty.");
            }
            return ValidationResult.Success();
        }
    }

    private readonly ScriptFileRunner _scriptFileRunner;

    public ScriptCommand(IFileSystem fileSystem,
                         State state,
                         ScriptFileRunner scriptFileRunner) : base(fileSystem, state)
    {
        _scriptFileRunner = scriptFileRunner;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context,
                                                    Arguments settings,
                                                    CancellationToken cancellationToken)
    {
        var scriptFile = Path.GetFullPath(settings.ScriptPath);
        if (!_fileSystem.FileExists(scriptFile))
        {
            return Exit($"File '{scriptFile}' does not exist.");
        }

        bool result = await _scriptFileRunner.RunAsync(scriptFile);

        return result ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
