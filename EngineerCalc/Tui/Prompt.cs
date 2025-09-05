using Spectre.Console;

namespace EngineerCalc.Tui;

internal static class Prompt
{
    public static void DoPrompt(State state)
    {
        var rule = new Rule($"[bold yellow]{state.Culture.ThreeLetterISOLanguageName}[/] | [orangered1]{DateTime.Now.ToShortTimeString().EscapeMarkup()}[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(rule);
    }
}
