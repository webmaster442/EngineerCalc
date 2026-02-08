//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Extensions;

internal static class StringExtensions
{
    public static int ToWordIndex(this string line, int currentPosition)
    {
        if (string.IsNullOrEmpty(line))
            return 0;

        int end = Math.Min(currentPosition, line.Length);
        int wordIndex = 0;
        for (int i = 0; i < end; i++)
        {
            if (line[i] == ' ') wordIndex++;
        }

        return wordIndex;

    }
}
