//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.VisualBasic;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace EngineerCalc.Tui.Sixel;

/// <summary>
/// Represents a renderable image in sixel format.
/// </summary>
public class SixelImage : Renderable
{
    private static readonly IResampler _defaultResampler = KnownResamplers.Bicubic;

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

    /// <summary>
    /// Gets or sets the <see cref="IResampler"/> that should
    /// be used when scaling the image. Defaults to bicubic sampling.
    /// </summary>
    public IResampler? Resampler { get; set; }

    internal SixLabors.ImageSharp.Image<Rgba32> Image { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SixelImage"/> class.
    /// </summary>
    /// <param name="filename">The image filename.</param>
    public SixelImage(string filename)
    {
        Image = SixLabors.ImageSharp.Image.Load<Rgba32>(filename);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SixelImage"/> class.
    /// </summary>
    /// <param name="data">Buffer containing an image.</param>
    public SixelImage(ReadOnlySpan<byte> data)
    {
        Image = SixLabors.ImageSharp.Image.Load<Rgba32>(data);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SixelImage"/> class.
    /// </summary>
    /// <param name="data">Stream containing an image.</param>
    public SixelImage(Stream data)
    {
        Image = SixLabors.ImageSharp.Image.Load<Rgba32>(data);
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

        // Draw a transparent renderable to take up the space the sixel is drawn in.
        // This allows Spectre.Console to render the image and not write overtop of it with space characters while padding panel borders etc.
        var canvas = new Canvas(cellWidth, cellHeight)
        {
            MaxWidth = cellWidth,
            Scale = false,
        };

        SixLabors.ImageSharp.Size size = new(cellWidth * maxWidth, cellHeight * options.ConsoleSize.Height);

        //mutate image
        Image.Mutate(ctx =>
        {
            ctx.Resize(new ResizeOptions()
            {
                Sampler = _defaultResampler,
                Size = size,
                Mode = ResizeMode.Manual,
                TargetRectangle = new SixLabors.ImageSharp.Rectangle(new SixLabors.ImageSharp.Point(), size),
                PremultiplyAlpha = true,
            });

            // Sixel supports 256 colors max
            ctx.Quantize(new OctreeQuantizer(new()
            {
                MaxColors = options.ColorSystem == ColorSystem.Standard ? 16 : 256,
            }));
        });

        yield return Segment.Control(SixelEncoder.Encode(Image));
    }
}
