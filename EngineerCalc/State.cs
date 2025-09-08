using System.Globalization;

namespace EngineerCalc;

internal sealed class State
{
    public CultureInfo Culture { get; set; }

    public ParseMode ParseMode { get; set; }

    public State()
    {
        ParseMode = ParseMode.Infix;
        Culture = CultureInfo.CurrentUICulture;
    }
}
