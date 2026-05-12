//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;
using EngineerCalc.Tui.Oxyplot;
using EngineerCalc.Tui.Sixel;

using OxyPlot;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class PlotCommand : FileSystemCommand<PlotCommand.Settings>
{
    private readonly IEvaluatorApi _api;

    public sealed class Settings : ExpressionCommandSettings
    {
        [CommandOption("-t|--title")]
        [Description("Title of the plot.")]
        public string Title { get; set; } = "Plot";

        [CommandOption("-x|--xlabel")]
        [Description("Label for the X axis.")]
        public string XLabel { get; set; } = "X";

        [CommandOption("-y|--ylabel")]
        [Description("Label for the Y axis.")]
        public string YLabel { get; set; } = "Y";

        [CommandOption("--min")]
        [Description("Minimum value of X.")]
        public double Min { get; set; } = 0;

        [CommandOption("--max")]
        [Description("Maximum value of X.")]
        public double Max { get; set; } = 100;

        [CommandOption("-o|--output")]
        [Description("Output file name for the plot (SVG format).")]
        public string FileName { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (Min >= Max)
                return ValidationResult.Error("Min must be less than Max.");

            if (string.IsNullOrWhiteSpace(Title))
                return ValidationResult.Error("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(XLabel))
                return ValidationResult.Error("XLabel cannot be empty.");

            if (string.IsNullOrWhiteSpace(YLabel))
                return ValidationResult.Error("YLabel cannot be empty.");

            return base.Validate();
        }
    }

    public PlotCommand(State state, IFileSystem fileSystem, IEvaluatorApi api)
        : base(fileSystem, state)
    {
        _api = api;
    }

    private static List<DataPoint> RenderDataPoints(IExpression expression, Settings settings)
    {
        static double CalculateSteps(double min, double max)
        {
            const double hdHorizontalResolution = 1920;
            double range = max - min;
            return range / (hdHorizontalResolution * 2);
        }

        double step = CalculateSteps(settings.Min, settings.Max);

        expression = expression.Simplify().Simplify();

        double value = settings.Min;
        List<DataPoint> points = new();
        while (value <= settings.Max)
        {
            value += step;
            Result computed = expression.Evaluate(new VariablesAndConstantsCollection
                {
                    { "x", Result.FromDouble(value) }
                });

            if (computed.TypeState != TypeState.Double)
                throw new InvalidOperationException("Expression did not evaluate to a numeric value.");

            points.Add(new DataPoint(value, computed.CastToDouble()));
        }
        return points;
    }

    protected override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            IExpression expression = _state.ParseMode == ParseMode.Infix
                ? _api.Parse(settings.Expression)
                : _api.ParseRpn(settings.Expression);

            var model = new PlotModel();
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = settings.XLabel
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = settings.YLabel
            });
            model.Title = settings.Title;

            var series = new OxyPlot.Series.LineSeries
            {
                ItemsSource = RenderDataPoints(expression, settings),
                Title = settings.Expression
            };

            model.Series.Add(series);

            if (!string.IsNullOrEmpty(settings.FileName))
            {
                Export(model, settings.FileName);
            }
            else
            {
                Display(model);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {ex.Message}[/]");
            return Task.FromResult(ExitCodes.GeneralError);
        }

        return Task.FromResult(ExitCodes.Success);
    }

    private static void Display(PlotModel model)
    {
        var (cellWidth, cellHeight) = SixelEncoder.GetCellSize();
        (int renderWidth, int renderHeght) = (cellWidth * AnsiConsole.Profile.Width, cellHeight * AnsiConsole.Profile.Height);

        var exporter = new PngExporter(width: renderWidth, height: renderHeght, resolution: 96);
        model.Background = OxyColors.White;
        using (var memStream = new MemoryStream())
        {
            exporter.Export(model, memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            AnsiConsole.AlternateScreen(() =>
            {
                SixelImage img = new(memStream);
                AnsiConsole.Write(img);
                Console.ReadLine();
            });
        }
    }

    private void Export(PlotModel model, string fileName)
    {
        var fullPath = GetFullPath(fileName);
        var exporter = new SvgExporter
        {
            Width = 1920,
            Height = 1080,
        };
        using var stream = _fileSystem.Create(fullPath);
        exporter.Export(model, stream);
        AnsiConsole.MarkupLine($"[green]Plot exported to {fullPath}[/]");
    }
}
