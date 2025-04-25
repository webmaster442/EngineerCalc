using System.Text;
using System.Web;

using EngineerCalc.Calculator;

namespace EngineerCalc.Endpoints;

internal static class Extensions
{
    public static StringBuilder BeginElement(this StringBuilder sb, string element, string @class)
        => sb.Append($"<{element} class=\"{@class}\">");

    public static StringBuilder EndElement(this StringBuilder sb, string element)
        => sb.Append($"</{element}>");
}

internal sealed class HtmlBuilder
{
    private readonly StringBuilder _builder = new StringBuilder(1024);

    public HtmlBuilder Reset()
    {
        _builder.Clear();
        return this;
    }

    public HtmlBuilder Exception(Exception ex)
    {
        _builder.BeginElement("p", CssClassNames.Error)
                .Append(HttpUtility.HtmlEncode(ex.Message))
                .EndElement("p");
#if DEBUG
        _builder.BeginElement("pre", "error details")
                .Append(HttpUtility.HtmlEncode(ex.StackTrace))
                .EndElement("pre");
#endif
        return this;
    }

    public HtmlBuilder AddResult(string result)
    {
        _builder.BeginElement("p", CssClassNames.Result)
                .Append(HttpUtility.HtmlEncode(result))
                .EndElement("p");
        return this;
    }

    public HtmlBuilder AddHeader(string header, int level = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(level, 6);

        _builder.Append($"<h{level}>")
                .Append(HttpUtility.HtmlEncode(header))
                .Append($"</h{level}>");
        return this;
    }

    public HtmlBuilder AddCode(string code, string @class)
    {
        _builder.BeginElement("code", @class)
                .Append(HttpUtility.HtmlEncode(code))
                .EndElement("code");
        return this;
    }
      

    public HtmlBuilder AddTable(TableData tableData)
    {
        _builder.BeginElement("table", "table");
        _builder.AppendLine("<tr>");
        foreach (var header in tableData.HeaderColumns)
        {
            _builder.Append("<th>")
                    .Append(HttpUtility.HtmlEncode(header))
                    .AppendLine("</th>");
        }
        _builder.AppendLine("</tr>");

        foreach (var row in tableData.TableContent)
        {
            _builder.AppendLine("<tr>");
            foreach (var colum in row)
            {
                _builder.Append("<td>")
                        .Append(HttpUtility.HtmlEncode(colum))
                        .AppendLine("</td>");
            }
            _builder.AppendLine("</tr>");
        }
        _builder.EndElement("table");
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
