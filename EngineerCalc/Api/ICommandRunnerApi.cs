using EngineerCalc.Models.XmlDoc;

namespace EngineerCalc.Api;

internal interface ICommandRunnerApi
{
    IDictionary<string, Command> KnownCommands { get; }
}
