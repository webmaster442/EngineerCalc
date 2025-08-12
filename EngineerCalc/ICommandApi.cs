namespace EngineerCalc;

internal interface ICommandApi
{
    void Exit(int exitCode);
    void Clear();
    IEvaluatorApi Evaluator { get; }
}
