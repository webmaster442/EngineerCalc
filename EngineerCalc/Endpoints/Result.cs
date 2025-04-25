using System.Net.Mime;
using System.Web;

namespace EngineerCalc.Endpoints;

internal sealed record class Result
{
    public static readonly Result Default = new();

    public string Message { get; private set; } = string.Empty;
    public bool IsSuccess { get; private set; }

    public static Result SuccessToHtml(string message)
    {
        return new Result
        {
            Message = $"<p class=\"{CssClassNames.Result}\">{HttpUtility.HtmlEncode(message)}</p>",
            IsSuccess = true
        };

    }

    public static Result ErrorToHtml(string message)
    {
        return new Result
        {
            Message = $"<p class=\"{CssClassNames.Error}\">{HttpUtility.HtmlEncode(message)}</p>",
            IsSuccess = false
        };
    }

    public static Result FromSuccess(object message)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message.ToString() ?? ""
        };
    }

    public static Result FromError(object message)
    {
        return new Result
        {
            Message = message.ToString() ?? "",
            IsSuccess = false
        };
    }

    public IResult ToIResult(string mediaType = MediaTypeNames.Text.Html)
    {
        return IsSuccess 
            ? Results.Content(Message, mediaType) 
            : Results.InternalServerError(Message);
    }
}
