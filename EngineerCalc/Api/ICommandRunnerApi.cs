//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Models.XmlDoc;

namespace EngineerCalc.Api;

internal interface ICommandRunnerApi
{
    IDictionary<string, Command> KnownCommands { get; }
}
