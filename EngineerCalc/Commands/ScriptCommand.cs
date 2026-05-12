//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;
using EngineerCalc.Infrastructure;
using EngineerCalc.Models;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ScriptCommand : FileSystemCommand<ScriptCommand.Arguments>
{
    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<scriptPath>")]
        [Description("The path to the script file to execute.")]
        [TypeConverter(typeof(FilePathConverter))]
        public FilePath ScriptPath { get; set; } = string.Empty;

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
        var scriptFile = GetFullPath(settings.ScriptPath);
        if (!_fileSystem.FileExists(scriptFile))
        {
            return Exit($"File '{scriptFile}' does not exist.");
        }

        bool result = await _scriptFileRunner.RunAsync(scriptFile);

        return result ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
