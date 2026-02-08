namespace EngineerCalc.Tui;

public interface IConsoleDriver
{
    int WindowWidth { get; }
    int CursorLeft { get; set; }
    void Write(string text);
    void Write(char c);
    void WriteLine();
    ConsoleKeyInfo ReadKey(bool intercept);
    void SetCursorPosition(int left, int top);
    (int left, int top) GetCursorPosition();
    (ConsoleColor background, ConsoleColor foreground) GetColors();
    void SetColors(ConsoleColor background, ConsoleColor foreground);
}
