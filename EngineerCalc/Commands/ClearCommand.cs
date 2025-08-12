namespace EngineerCalc.Commands;

internal sealed class ClearCommand : ICommand
{
    public Task Execute(ICommandApi api, string[] args)
    {
        api.Clear();
        return Task.CompletedTask;
    }
}