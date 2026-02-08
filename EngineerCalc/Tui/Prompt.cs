//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;

namespace EngineerCalc.Tui;

internal static class Prompt
{
    public static void DoPrompt(State state)
    {
        var text = $"[bold yellow]{state.Culture.ThreeLetterISOLanguageName}[/] | [green]{state.ParseMode}[/] | [orangered1]{DateTime.Now.ToShortTimeString().EscapeMarkup()}[/]";
        AnsiConsole.Write("╔═══ ");
        AnsiConsole.MarkupLine(text);
    }
}
