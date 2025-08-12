namespace EngineerCalc;

internal interface ICommand
{
    public Task Execute(ICommandApi api, string[] args);
}
