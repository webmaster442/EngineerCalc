using Spectre.Console;

namespace EngineerCalc.Tui;

internal static class Prompt
{
    public static void DoPrompt()
    {
        var msg = $"[orangered1]{DateTime.Now.ToShortTimeString().EscapeMarkup()}[/]";
        var rule = new Rule(msg)
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(rule);
    }
}
