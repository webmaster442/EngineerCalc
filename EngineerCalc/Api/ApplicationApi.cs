namespace EngineerCalc.Api;

public sealed class ApplicationApi : IApplicationApi
{
    public void Exit(int exitCode)
    {
        Environment.Exit(exitCode);
    }
    public void Clear()
    {
        Console.Clear();
    }
}