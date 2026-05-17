//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using OxyPlot;

using SkiaSharp;

namespace EngineerCalc.Tui.Oxyplot;

public static class SkiaExtensions
{
    /// <summary>
    /// Converts a <see cref="OxyColor"/> to a <see cref="SKColor"/>;
    /// </summary>
    /// <param name="color">The <see cref="OxyColor"/>.</param>
    /// <returns>The <see cref="SKColor"/>.</returns>
    public static OxyColor ToOxyColor(this SKColor color)
    {
        return OxyColor.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
    }

    /// <summary>
    /// Converts a <see cref="SKColor"/> to a <see cref="OxyColor"/>;
    /// </summary>
    /// <param name="color">The <see cref="SKColor"/>.</param>
    /// <returns>The <see cref="OxyColor"/>.</returns>
    public static SKColor ToSKColor(this OxyColor color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }
}
