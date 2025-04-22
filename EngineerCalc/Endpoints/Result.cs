using System.Net.Mime;

namespace EngineerCalc.Endpoints;

internal sealed record class Result
{
    public static readonly Result Default = new();

    public string Message { get; private set; } = string.Empty;
    public bool IsSuccess { get; private set; }

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
