//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using SkiaSharp;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace EngineerCalc.Tui.Sixel;

/// <summary>
/// Represents a renderable image in sixel format.
/// </summary>
internal sealed class SixelImage : Renderable, IDisposable
{
    /// <summary>
    /// Gets the image width.
    /// </summary>
    public int Width => Image.Width;

    /// <summary>
    /// Gets the image height.
    /// </summary>
    public int Height => Image.Height;

    /// <summary>
    /// Gets or sets the render width of the canvas.
    /// </summary>
    public int? MaxWidth { get; set; }

    internal SKBitmap Image { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SixelImage"/> class.
    /// </summary>
    /// <param name="data">Stream containing an image.</param>
    public SixelImage(Stream data)
    {
        Image = SKBitmap.Decode(data);
    }

    public void Dispose()
    {
        Image.Dispose();
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (Width < 0)
        {
            throw new InvalidOperationException("Pixel width must be greater than zero.");
        }

        var width = MaxWidth ?? Width;
        if (maxWidth < width * Width)
        {
            return new Measurement(maxWidth, maxWidth);
        }

        return new Measurement(width * Width, width * Width);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (MaxWidth != null && MaxWidth < maxWidth)
        {
            maxWidth = MaxWidth.Value;
        }

        var (cellWidth, cellHeight) = SixelEncoder.GetCellSize();


        SKSizeI size = new(cellWidth * maxWidth, cellHeight * options.ConsoleSize.Height);

        using var resized = Image.Resize(size, SKSamplingOptions.Default);

        var data = Octree.Quantize(resized, options.ColorSystem == ColorSystem.Standard ? 16 : 256);

        yield return Segment.Control(SixelEncoder.Encode(data, resized.Width, resized.Height));
    }
}
