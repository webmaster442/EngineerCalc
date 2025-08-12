namespace EngineerCalc.Tui;

public sealed class SystemConsoleDriver : IConsoleDriver
{
    public int WindowWidth
        => Console.WindowWidth;

    public int CursorLeft
    {
        get => Console.CursorLeft; 
        set => Console.CursorLeft = value;
    }
    
    public void Write(string text)
        => Console.Write(text);
    
    public void WriteLine()
        => Console.WriteLine();
    
    public ConsoleKeyInfo ReadKey(bool intercept)
        => Console.ReadKey(intercept);

    public void SetCursorPosition(int left, int top)
        => Console.SetCursorPosition(left, top);

    public (int left, int top) GetCursorPosition()
        => Console.GetCursorPosition();

    public (ConsoleColor background, ConsoleColor foreground) GetColors()
        => (Console.BackgroundColor, Console.ForegroundColor);

    public void SetColors(ConsoleColor background, ConsoleColor foreground)
    {
        Console.BackgroundColor = background;
        Console.ForegroundColor = foreground;
    }
}