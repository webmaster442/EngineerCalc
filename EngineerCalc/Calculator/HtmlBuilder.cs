using System.Text;

namespace EngineerCalc.Calculator;

internal sealed class HtmlBuilder
{
    private readonly StringBuilder _builder = new StringBuilder(1024);

    public HtmlBuilder Reset()
    {
        _builder.Clear();
        return this;
    }

    private void WrapInResponse(Action action)
    {
        _builder.Append("<div class=\"response\">");
        action.Invoke();
        _builder.Append("</div>");
    }

    public HtmlBuilder Exception(string input, Exception ex)
    {
        WrapInResponse(() =>
        {
            _builder
                .Append($"<p class=\"echo\">{input}</p>")
                .Append($"<p class=\"error\">{ex.Message}</p>")
                .Append($"<pre>{ex.StackTrace}</pre>");
        });
        return this;
    }

    public HtmlBuilder AddResult(string input, string result)
    {
        WrapInResponse(() =>
        {
            _builder
                .Append($"<p class=\"echo\">{input}</p>")
                .Append($"<p class=\"result\">{result}</p>");
        });
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
