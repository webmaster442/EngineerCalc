using System.Net.Mime;
using System.Web;

using Microsoft.AspNetCore.Http.HttpResults;

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
        int statuscode = IsSuccess ? 200 : 400;
        return Results.Content(Message, mediaType, statusCode: statuscode); 
    }
}
