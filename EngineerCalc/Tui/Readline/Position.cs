//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Tui.Readline;

internal readonly struct Position
{
    public int StringPosition { get; }
    public int ScreenPosition { get; }

    public Position(int stringPosition, int screenPosition)
    {
        StringPosition = stringPosition;
        ScreenPosition = screenPosition;
    }

    public static Position operator ++(Position position)
        => new(position.StringPosition + 1, position.ScreenPosition + 1);

    public static Position operator --(Position position)
        => new(position.StringPosition - 1, position.ScreenPosition - 1);
}
