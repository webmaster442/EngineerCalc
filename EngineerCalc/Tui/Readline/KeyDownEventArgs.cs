//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Tui.Readline;

internal sealed class KeyDownEventArgs : EventArgs
{
    public KeyDownEventArgs(ConsoleKeyInfo current)
    {
        Key = current.Key;
        Modifiers = current.Modifiers;
    }

    public ConsoleKey Key { get; }
    public ConsoleModifiers Modifiers { get;  }
}
