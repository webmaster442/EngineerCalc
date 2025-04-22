using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using EngineerCalc.Calculator;

namespace EngineerCalc.Endpoints;

internal abstract class CommandBase<TSettings> : ICommand where TSettings : class, new()
{
    public abstract string Name { get; }

    public async Task<Result> Execute(State state, string[] arguments)
    {
        TSettings parsed = ParseArguments(arguments);
        return IsValid(parsed) ? await ExecuteInternal(state, parsed) : Result.FromSuccess(GetHelp(parsed));
    }

    protected abstract Task<Result> ExecuteInternal(State state, TSettings parsed);

    protected bool ParsedWithoutErrors { get; private set; }

    protected bool IsValid(TSettings parsed)
    {
        return ParsedWithoutErrors;
    }

    protected string GetHelp(TSettings parsed)
    {
        TableData table = new TableData();
        table.HeaderColumns.AddRange("Name", "Optional?", "Description");
        var properties = typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var builder = new HtmlBuilder().AddHeader($"Usage");
        StringBuilder usage = new(Name);
        usage.Append(' ');
        foreach (var property in properties)
        {
            var argData = property.GetCustomAttribute<ArgumentAttribute>();
            var required = property.GetCustomAttribute<RequiredAttribute>() != null;
            if (argData != null)
            {
                if (required)
                    usage.Append($"<{property.Name}> ");
                else
                    usage.Append($"[{property.Name}] ");

                table.TableContent.Add([property.Name, required ? "no" : "yes", argData.Description]);
            }
        }
        return builder.AddCode(usage.ToString(), "usage").AddHeader("Arguments", 2).AddTable(table).ToString();
    }

    private TSettings ParseArguments(string[] arguments)
    {
        static bool TryGet(string[] arguments, int index, [NotNullWhen(true)] out string? value)
        {
            if (index >= 0 && index <= arguments.Length - 1)
            {
                value = arguments[index];
                return true;
            }
            value = null;
            return false;
        }

        static bool TryConvert(string value, Type target, [NotNullWhen(true)] out object? result)
        {
            try
            {
                result = Convert.ChangeType(value, target);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        TSettings parsed = new();
        ParsedWithoutErrors = true;
        var properties = typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            bool isRequired = property.GetCustomAttribute<RequiredAttribute>() != null;
            ArgumentAttribute? argument = property.GetCustomAttribute<ArgumentAttribute>();
            if (argument == null)
                continue;

            if (TryGet(arguments, argument.Index, out string? value))
            {
                if (TryConvert(value, property.PropertyType, out object? converted))
                {
                    property.SetValue(parsed, converted);
                }
                else
                {
                    ParsedWithoutErrors = false;
                }
            }
            else if (isRequired)
            {
                ParsedWithoutErrors = false;
            }
        }
        return parsed;
    }

}
