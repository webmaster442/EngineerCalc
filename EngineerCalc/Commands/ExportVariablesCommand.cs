//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Text;

using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ExportVariablesCommand : FileSystemCommand<ExportVariablesCommand.Arguments>
{
    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<scriptPath>")]
        [Description("File to save variables to")]
        public string ScriptPath { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(ScriptPath))
            {
                return ValidationResult.Error("Script path cannot be empty.");
            }
            return ValidationResult.Success();
        }
    }

    private readonly IEvaluatorApi _evaluator;

    public ExportVariablesCommand(IFileSystem fileSystem, State state, IEvaluatorApi evaluator)
        : base(fileSystem, state)
    {
        _evaluator = evaluator;
    }

    private static string ToSerializableString(Result r)
    {
        static string FormatComplex(Complex complex) 
            => $"cplx({complex.Real.ToString("R", CultureInfo.InvariantCulture)}, {complex.Imaginary.ToString("R", CultureInfo.InvariantCulture)})";

        return r.TypeState switch
        {
            TypeState.NoResult => string.Empty,
            TypeState.Boolean => r.CastToBoolean().ToString(CultureInfo.InvariantCulture),
            TypeState.Integer => r.CastToBigInteger().ToString(CultureInfo.InvariantCulture),
            TypeState.Double => r.CastToDouble().ToString("R", CultureInfo.InvariantCulture),
            TypeState.Fraction => r.CastToFraction().ToString(CultureInfo.InvariantCulture),
            TypeState.Complex => FormatComplex(r.CastToComplex()),
            TypeState.Array => $"array({string.Join(",", r.CastToArray().Select(x => x.ToString("R", CultureInfo.InvariantCulture)))})",
            TypeState.String => $"'{r.CastToString().Replace("'", "\\'")}'",
            _ => throw new InvalidOperationException("Unknown result type state."),
        };
    }

    protected override async Task<int> ExecuteAsync(CommandContext context,
                                              Arguments settings,
                                              CancellationToken cancellationToken)
    {
        var filePath = GetFullPath(settings.ScriptPath);
        using var writer = new StreamWriter(_fileSystem.Create(filePath), Encoding.UTF8);

        foreach (var variable in _evaluator.VariablesAndConstants.Variables())
        {
            await writer.WriteLineAsync($"{variable.Key}={ToSerializableString(variable.Value)}");
        }

        return ExitCodes.Success;
    }
}
