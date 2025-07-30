using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerCalc.Tui.Readline;

internal struct CursorPosition
{
    public int TextPosition { get; set; }
    public int ScreenPosition { get; set; }
}

internal class Readline
{
    private readonly IConsole _console;
    private readonly StringBuilder _buffer;
    private CursorPosition _state;

    public Readline(IConsole console)
    {
        _console = console;
        _buffer = new StringBuilder(console.WindowWidth);
    }

    private void RewriteFrom(CursorPosition position)
    {
        _console.SetPosition(position.ScreenPosition);
        _buffer.ToString(position.TextPosition, 5);
    }
}