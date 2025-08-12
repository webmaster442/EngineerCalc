namespace EngineerCalc.Tui;

internal sealed class Statusbar
{
    private readonly IConsoleDriver _consoleDriver;

    public Statusbar(IConsoleDriver consoleDriver)
    {
        _consoleDriver = consoleDriver;
    }

    public Statusbar() : this(new SystemConsoleDriver())
    {
    }

    public void Render(string str)
    {
        var (left, top) = _consoleDriver.GetCursorPosition();
        _consoleDriver.SetCursorPosition(0, Console.WindowHeight - 1);
        var (background, foreground) = _consoleDriver.GetColors();
        _consoleDriver.SetColors(ConsoleColor.Blue, ConsoleColor.White);
        _consoleDriver.Write(str);
        _consoleDriver.Write(new string(' ', _consoleDriver.WindowWidth - str.Length));
        _consoleDriver.SetColors(background, foreground);
        _consoleDriver.SetCursorPosition(left, top);
    }
}
