//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using SkiaSharp;

namespace EngineerCalc.Tui.Sixel;

internal static class SixelEncoder
{
    private const char SIXELEMPTY = '?';
    private const char SIXELCOLORSTART = '#';
    private const char SIXELREPEAT = '!';
    private const char SIXELDECGCR = '$';
    private const char SIXELDECGNL = '-';
    private const string SIXELSTART = "\eP0;1q";
    private const string SIXELEND = "\e\\";
    private const string SIXELTRANSPARENTCOLOR = "#0;2;0;0;0";
    private const string SIXELRASTERATTRIBUTES = "\"1;1;";

    private static void AddColorToPalette(this StringBuilder sixelBuilder, SKColor pixel, int colorIndex)
    {
        var r = (int)Math.Round(pixel.Red / 255.0 * 100);
        var g = (int)Math.Round(pixel.Green / 255.0 * 100);
        var b = (int)Math.Round(pixel.Blue / 255.0 * 100);

        sixelBuilder.Append(SIXELCOLORSTART)
                    .Append(colorIndex)
                    .Append(";2;")
                    .Append(r)
                    .Append(';')
                    .Append(g)
                    .Append(';')
                    .Append(b);
    }


    private static void AppendRepeatEntry(this StringBuilder sixelBuilder,
                                          int color,
                                          int repeatCounter,
                                          char e)
    {
        sixelBuilder.Append(SIXELCOLORSTART)
                    .Append(color)
                    .Append(SIXELREPEAT)
                    .Append(repeatCounter)
                    .Append(color != 0 ? e : SIXELEMPTY);
    }

    private static void AppendSixelEntry(this StringBuilder sixelBuilder, int color, char e)
    {
        sixelBuilder.Append(SIXELCOLORSTART)
                    .Append(color)
                    .Append(color != 0 ? e : SIXELEMPTY);
    }

    private static void StartSixel(this StringBuilder sixelBuilder, int width, int height)
    {
        sixelBuilder.Append(SIXELSTART)
                    .Append(SIXELRASTERATTRIBUTES)
                    .Append(width)
                    .Append(';')
                    .Append(height)
                    .Append(SIXELTRANSPARENTCOLOR);
    }

    private static string GetControlSequenceResponse(string controlSequence)
    {
        char? c;
        var response = string.Empty;

        System.Console.Write($"\e{controlSequence}");
        do
        {
            c = System.Console.ReadKey(true).KeyChar;
            response += c;
        }
        while (c != 'c' && System.Console.KeyAvailable);

        return response;
    }

    public static (int cellWidth, int cellHeight) GetCellSize()
    {
        var response = GetControlSequenceResponse("[16t");

        try
        {
            var parts = response.Split(';', 't');
            return (cellWidth: int.Parse(parts[2]), cellHeight: int.Parse(parts[1]));
        }
        catch
        {
            // Return the default Windows Terminal size if we can't get the size from the terminal.
            return (cellWidth: 10, cellHeight: 20);
        }
    }

    public static string Encode(SKColor[] data, int width, int height)
    {
        static int AllocateSize(int width, int height)
            => (width / 6) * height;

        var sixelBuilder = new StringBuilder(AllocateSize(width, height));
        var palette = new Dictionary<SKColor, int>();
        var colorCounter = 1;
        sixelBuilder.StartSixel(width, height);

        for (var y = 0; y < height; y++)
        {
            // The way sixel works, this bitshift starting from the SIXELEMPTY constant
            // will give us the correct character to use for the current row.
            // Every six rows we swap back to the "empty character + 1" after adding a newline
            // character to the string.
            var c = (char)(SIXELEMPTY + (1 << (y % 6)));
            var lastColor = -1;
            var repeatCounter = 0;
            for (int x = 0; x < width; x++)
            {
                var pixel = data[y * width + x];
                if (!palette.TryGetValue(pixel, out var colorIndex))
                {
                    colorIndex = colorCounter++;
                    palette[pixel] = colorIndex;
                    sixelBuilder.AddColorToPalette(pixel, colorIndex);
                }
                var colorId = colorIndex;
                if (colorId == lastColor || repeatCounter == 0)
                {
                    lastColor = colorId;
                    repeatCounter++;
                    continue;
                }
                if (repeatCounter > 1)
                {
                    sixelBuilder.AppendRepeatEntry(lastColor, repeatCounter, c);
                }
                else
                {
                    sixelBuilder.AppendSixelEntry(lastColor, c);
                }
                lastColor = colorId;
                repeatCounter = 1;
            }
            if (repeatCounter > 1)
            {
                sixelBuilder.AppendRepeatEntry(lastColor, repeatCounter, c);
            }
            else
            {
                sixelBuilder.AppendSixelEntry(lastColor, c);
            }
            sixelBuilder.Append(SIXELDECGCR);
            if (y % 6 == 5)
            {
                sixelBuilder.Append(SIXELDECGNL);
            }
        }
        sixelBuilder.Append(SIXELEND);
        return sixelBuilder.ToString();
    }
}
