using System.Text;
using System.Web;

namespace EngineerCalc.Calculator;

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
        _builder.BeginElement("p", "error")
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
        _builder.BeginElement("p", "result")
                .Append(HttpUtility.HtmlEncode(result))
                .EndElement("p");
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
