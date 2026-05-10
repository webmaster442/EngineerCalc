//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using DynamicEvaluator.Documentation;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class FunctionsCommand : Command<FunctionsCommand.Arguments>
{
    public class Arguments : CommandSettings
    {
        [Description("The name of the function to get documentation for.")]
        [CommandArgument(0, "[function-name]")]
        public string FunctionName { get; set; } = string.Empty;
    }

    internal sealed class ConsoleFormatter : IDocumentFormatter
    {
        public string FormatDescription(string description)
            => $"    {description}";

        public string FormatExample(string example)
            => $"    [italic invert]{example.EscapeMarkup()}[/]";

        public string FormatName(string name)
            => $"[bold green]{name.EscapeMarkup()}[/]";

        public string FormatSectionTitle(string title)
            => $"[bold yellow]{title.EscapeMarkup()}[/]";

        public string FormatSummary(string summary)
            => $"[italic]{summary.EscapeMarkup()}[/]";

        public string FormatTypes(string[] types)
        {
            var typeList = string.Join(", ", types.Select(t => $"[italic red]{t.EscapeMarkup()}[/]"));
            return $"    Supported types: {typeList}";
        }
    }

    protected override int Execute(CommandContext context, Arguments settings, CancellationToken cancellationToken)
    {
        DocumentationProvider documentation = new();

        if (string.IsNullOrEmpty(settings.FunctionName))
        {
            AnsiConsole.WriteLine("Available functions: ");
            AnsiConsole.WriteLine("To get details of a function use .functions <functionname>");
            var docTable = documentation.OrderBy(x => x.Name).Select(x => new string[]
            {
                x.Name,
                x.Summary
            });

            var table = new Table();
            table.AddColumns("Function", "Description");
            foreach (var row in docTable)
            {
                table.AddRow(row);
            }
            AnsiConsole.Write(table);
            return ExitCodes.Success;
        }

        if (!documentation.IsDocumented(settings.FunctionName))
        {
            throw new InvalidOperationException($"No documentation found for: {settings.FunctionName}");
        }

        var documentRenderer = new DocumentationRenderer(new ConsoleFormatter(), documentation);
        AnsiConsole.MarkupLine(documentRenderer.GetDocumentation(settings.FunctionName));

        return ExitCodes.Success;
    }
}
