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

    protected override int Execute(CommandContext context, Arguments settings, CancellationToken cancellationToken)
    {
        DocumentationProvider documentation = new();

        if (string.IsNullOrEmpty(settings.FunctionName))
        {
            AnsiConsole.WriteLine("Availabe functions: ");
            AnsiConsole.WriteLine("To get details of a function use .functions <functionname>");
            var docTable = documentation.OrderBy(x => x.FunctionName).Select(x => new string[]
            {
                x.FunctionName,
                x.Description
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

        var doc = documentation.GetDocumentation(settings.FunctionName);

        AnsiConsole.MarkupLine($"[yellow]Function:[/] {doc.FunctionName.EscapeMarkup()}");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green italic]  {doc.Description.EscapeMarkup()}[/]");
        foreach (var example in doc.Examples)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[blue italic]{example.EscapeMarkup()}[/]");
        }
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Paramaters");
        AnsiConsole.WriteLine();
        foreach (var argument in doc.Arguments)
        {
            AnsiConsole.MarkupLine($"[yellow]{argument.Name}[/]");
            AnsiConsole.MarkupLine($"  [green italic]{argument.Description.EscapeMarkup()}[/]");
            AnsiConsole.WriteLine();
        }

        return ExitCodes.Success;
    }
}
