namespace EngineerCalc.Tui;

internal interface IConsole
{
    int WindowWidth { get; }

    void SetPosition(int screenPosition);
}
