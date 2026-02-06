using System.ComponentModel;
using System.Diagnostics;

using DynamicEvaluator;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using OxyPlot;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class PlotCommand : Command<PlotCommand.Settings>
{
    private readonly State _state;
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
        public string FileName { get; set; } = Path.Combine(Environment.CurrentDirectory, "plot.svg");

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

            if (string.IsNullOrWhiteSpace(FileName))
                return ValidationResult.Error("FileName cannot be empty.");

            var directory = Path.GetDirectoryName(FileName);

            if (directory == null || !Directory.Exists(directory))
                return ValidationResult.Error("Directory for the output file does not exist.");

            return base.Validate();
        }
    }

    public PlotCommand(State state, IEvaluatorApi api)
    {
        _state = state;
        _api = api;
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
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

            double step = CalculateSteps(settings.Min, settings.Max);

            expression = expression.Simplify().Simplify();

            double value = settings.Min;
            List<DataPoint> points = new();
            while (value <= settings.Max)
            {
                value += step;
                dynamic computed = expression.Evaluate(new VariablesAndConstantsCollection
                {
                    { "x", value }
                });
                if (computed is double y)
                {
                    points.Add(new DataPoint(value, y));
                }
                else
                {
                    throw new InvalidOperationException("Expression did not evaluate to a numeric value.");
                }
            }

            var series = new OxyPlot.Series.LineSeries
            {
                ItemsSource = points,
                Title = settings.Expression
            };

            model.Series.Add(series);

            using (var stream = File.Create(settings.FileName))
            {
                var exporter = new SvgExporter
                { 
                    Width = 1920, 
                    Height = 1080,
                };
                exporter.Export(model, stream);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = settings.FileName,
                UseShellExecute = true
            });

        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {ex.Message}[/]");
            return ExitCodes.GeneralError;
        }

        return ExitCodes.Success;
    }

    private static double CalculateSteps(double min, double max)
    {
        const double hdHorizontalResolution = 1920;
        double range = max - min;
        return range / (hdHorizontalResolution * 2);
    }
}
