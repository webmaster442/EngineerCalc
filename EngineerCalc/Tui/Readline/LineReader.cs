//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace EngineerCalc.Tui.Readline;

internal sealed class LineReader
{
    private readonly CompleterState _completerState;
    private readonly IHistory? _history;
    private readonly IConsoleDriver _console;
    private readonly StringBuilder _buffer;
    private Position _currentPosition;
    private Position _startPosition;

    public LineReader(IConsoleDriver consoleDriver,
                      IHistory? history = null,
                      ICompleter? completer = null)
    {
        _console = consoleDriver;
        _buffer = new StringBuilder(_console.WindowWidth);
        _completerState = new(completer);
        _history = history;
        _currentPosition = new Position(0, 0);
    }

    public LineReader(ICompleter? completer = null) : this(new SystemConsoleDriver(), new InMemoryHistoryList(), completer)
    {
    }

    public string ReadLine(string prompt)
    {
        ConsoleKeyInfo current;
        _console.Write(prompt);
        _startPosition = new Position(0, _console.CursorLeft);
        _currentPosition = new Position(0, _console.CursorLeft);
        _history?.ResetIndex();
        do
        {
            current = _console.ReadKey(intercept: true);
            switch (current.Key)
            {
                case ConsoleKey.Backspace:
                    OnBackSpace();
                    break;
                case ConsoleKey.LeftArrow:
                    OnLeftArrow();
                    break;
                case ConsoleKey.RightArrow:
                    OnRightArrow();
                    break;
                case ConsoleKey.UpArrow:
                    OnUparrow();
                    break;
                case ConsoleKey.DownArrow:
                    OnDownArrow();
                    break;
                case ConsoleKey.Tab:
                    if (current.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        OnShiftTab();
                    else
                        OnTab();
                    break;
                default:
                    OnTextInput(current);
                    _completerState.Reset();
                    break;
            }

        }
        while (current.Key != ConsoleKey.Enter);
        _console.WriteLine();
        string line = _buffer.ToString();
        _history?.Add(line);
        _buffer.Clear();
        return line;
    }

    private void OnTab()
    {
        if (_completerState.TryGetNext(_buffer.ToString(), _currentPosition.StringPosition, out string completion))
        {
            ReplaceCurrentWord(completion);
        }
    }

    private void OnShiftTab()
    {
        if (_completerState.TryGetPrevious(_buffer.ToString(), _currentPosition.StringPosition, out string completion))
        {
            ReplaceCurrentWord(completion);
        }
    }

    private void ReplaceCurrentWord(string completion)
    {
        int wordStart = _currentPosition.StringPosition;
        while (wordStart > 0 
               && !char.IsWhiteSpace(_buffer[wordStart - 1]))
        {
            wordStart--;
        }

        int wordEnd = _currentPosition.StringPosition;
        while (wordEnd < _buffer.Length 
               && !char.IsWhiteSpace(_buffer[wordEnd]))
        {
            wordEnd++;
        }

        int wordLength = wordEnd - wordStart;

        // Remove the old word and insert the completion
        if (wordLength > 0)
        {
            _buffer.Remove(wordStart, wordLength);
        }
        _buffer.Insert(wordStart, completion);

        _currentPosition = new Position(
            wordStart + completion.Length,
            _startPosition.ScreenPosition + wordStart + completion.Length
        );

        ClearLine();
        RewriteText();
        _console.CursorLeft = _currentPosition.ScreenPosition;
    }

    private void OnTextInput(ConsoleKeyInfo current)
    {
        if (IsSpecialKey(current))
            return;

        _buffer.Insert(_currentPosition.StringPosition, current.KeyChar);
        ++_currentPosition;
        RewriteText();
    }

    private void OnBackSpace()
    {
        if (_currentPosition.StringPosition - 1 < 0)
            return;

        _buffer.Remove(_currentPosition.StringPosition - 1, 1);
        --_currentPosition;
        ClearLine();
        RewriteText();
    }

    private void OnLeftArrow()
    {
        if (_currentPosition.StringPosition - 1 < 0)
            return;

        --_currentPosition;
        _console.CursorLeft = _currentPosition.ScreenPosition;
    }

    private void OnRightArrow()
    {
        if (_currentPosition.StringPosition + 1 > _buffer.Length)
            return;

        ++_currentPosition;
        _console.CursorLeft = _currentPosition.ScreenPosition;
    }

    private void OnDownArrow()
    {
        if (_history == null || _history.Count <= 0)
            return;

        if (_history.Count == 1)
        {
            RewriteText(_history.Entries[0]);
        }
        else if (_history.CurrentIndex + 1 < _history.Count)
        {
            RewriteText(_history.Current);
            ++_history.CurrentIndex;
        }
    }

    private void OnUparrow()
    {
        if (_history == null || _history.Count <= 0)
            return;

        if (_history.Count == 1)
        {
            RewriteText(_history.Entries[0]);
        }
        else if (_history.CurrentIndex - 1 >= 0)
        {
            RewriteText(_history.Current);
            --_history.CurrentIndex;
        }
        if (_history.CurrentIndex < 0)
        {
            _history.CurrentIndex = 0;
        }
    }

    private static bool IsSpecialKey(ConsoleKeyInfo consoleKey)
        => consoleKey.KeyChar == '\0'
           || consoleKey.KeyChar == '\r'
           || consoleKey.KeyChar == '\n';

    private void RewriteText(string text)
    {
        ClearLine();
        _buffer.Clear().Append(text);
        RewriteText();
        _currentPosition = new Position(_buffer.Length, _startPosition.ScreenPosition + _buffer.Length);
        _console.CursorLeft = _currentPosition.ScreenPosition;
    }

    private void RewriteText()
    {
        if (_currentPosition.ScreenPosition - _console.CursorLeft == 1
            && _currentPosition.StringPosition == _buffer.Length)
        {
            _console.Write(_buffer[_buffer.Length - 1]);
            return;
        }

        _console.CursorLeft = _startPosition.ScreenPosition;
        _console.Write(_buffer.ToString());
        _console.CursorLeft = _currentPosition.ScreenPosition;
    }

    private void ClearLine()
    {
        _console.CursorLeft = _startPosition.ScreenPosition;
        _console.Write(new string(' ', _console.WindowWidth - _console.CursorLeft));
    }
}
