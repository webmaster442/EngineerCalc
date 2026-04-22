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
        string[] lines =
        [
            $"╭─ {GetDirectoryString(state.CurrentDirectory)}",
            $"╰─ [bold yellow]{state.Culture.ThreeLetterISOLanguageName}[/] | [green]{state.ParseMode}[/] | [orangered1]{DateTime.Now.ToShortTimeString().EscapeMarkup()}[/]"
        ];
        for (int i = 0; i < lines.Length -1; i++)
        {
            AnsiConsole.MarkupLine(lines[i]);
        }
        AnsiConsole.Markup(lines[^1]);
    }

    private static string GetDirectoryString(string currentDirectory)
    {
        int maxLength = Console.WindowWidth - 8;
        if (currentDirectory.Length > maxLength)
        {
            string part = currentDirectory[^maxLength..];
            return $"... {part}";
        }
        return currentDirectory;
    }
}
