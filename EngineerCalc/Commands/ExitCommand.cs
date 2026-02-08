//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Api;

using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ExitCommand : Command
{
    private readonly IApplicationApi _api;

    public ExitCommand(IApplicationApi api)
    {
        _api = api;
    }

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        _api.Exit(0);
        return ExitCodes.Success;
    }
}
