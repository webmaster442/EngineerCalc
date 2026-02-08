//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace EngineerCalc.Extensions;

internal static class StringExtensions
{
    extension(string line)
    {
        public int ToWordIndex(int currentPosition)
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

    extension(StringBuilder line)
    {
        public int ToWordIndex(int currentPosition)
        {
            if (line.Length == 0)
                return 0;
            int end = Math.Min(currentPosition, line.Length);
            int wordIndex = 0;
            for (int i = 0; i < end; i++)
            {
                if (line[i] == ' ') wordIndex++;
            }
            return wordIndex;
        }

        public IEnumerable<string> GetWords()
        {
            string[] words = line.ToString().Split(' ');
            return words.Where(w => !string.IsNullOrEmpty(w));
        }
    }
}
