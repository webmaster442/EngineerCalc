namespace EngineerCalc.Endpoints;

internal interface ICommand
{
    string Name { get; }
    Task<Result> Execute(State state, string[] arguments);
}
