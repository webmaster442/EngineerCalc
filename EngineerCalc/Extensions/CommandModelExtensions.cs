//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Models.XmlDoc;

namespace EngineerCalc.Extensions;

internal static class CommandModelExtensions
{
    public static int MapWordIndexAsArgsIndex(this Command command, string[] words, int wordIndex)
    {
        var argumentPositions = command.Parameters.Arguments.Select(a => a.Position).ToArray();
        
        if (argumentPositions.Length == 0 || words[wordIndex].StartsWith('-'))
            return int.MaxValue;

        string currentWord = words[wordIndex];

        int calculated = 0;
        bool previousWasOption = false;

        for (int i = 1; i < wordIndex; i++)
        {
            if (words[i].StartsWith("-"))
            {
                previousWasOption = true;
            }
            else if (!previousWasOption)
            {
                calculated++;
            }
            else
            {
                previousWasOption = false;
            }
        }

        if (argumentPositions.Contains(calculated))
            return calculated;

        return int.MaxValue;
    }
}
