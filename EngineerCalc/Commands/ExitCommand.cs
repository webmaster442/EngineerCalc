namespace EngineerCalc.Commands;

internal sealed class ExitCommand : ICommand
{
    public Task Execute(ICommandApi api, string[] args)
    {
        api.Exit(0);
        return Task.CompletedTask;
    }
}
