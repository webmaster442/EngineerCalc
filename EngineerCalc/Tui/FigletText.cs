using Spectre.Console;

namespace EngineerCalc.Tui;

internal static class FigletTextFactory
{

    public static FigletText AnsiShadow(string text)
    {
        using var fontStream = typeof(FigletTextFactory).Assembly
            .GetManifestResourceStream("EngineerCalc.Tui.FigletFonts.Ansi_Shadow.flf");

        if (fontStream is null)
            throw new InvalidOperationException("Could not load the Ansi Shadow font.");

        var font = FigletFont.Load(fontStream);

        return new FigletText(font, text);
    }
}
