using DynamicEvaluator;

namespace EngineerCalc;

internal class CommandApi : ICommandApi
{
    public IEvaluatorApi Evaluator { get; }

    public void Exit(int exitCode)
        => Environment.Exit(exitCode);

    public void Clear()
        => Console.Clear();

    internal class EvaluatorApi : IEvaluatorApi
    {
        private readonly ExpressionFactory _expressionFactory;

        public VariablesAndConstantsCollection VariablesAndConstants { get; }

        public IExpression Parse(string expression)
        {
            throw new NotImplementedException();
        }

        public EvaluatorApi(VariablesAndConstantsCollection variablesAndConstants,
                            ExpressionFactory expressionFactory)
        {
            VariablesAndConstants = variablesAndConstants;
            _expressionFactory = expressionFactory;
        }
    }

    public CommandApi(VariablesAndConstantsCollection variablesAndConstants,
                      ExpressionFactory expressionFactory)
    {
        Evaluator = new EvaluatorApi(variablesAndConstants, expressionFactory);
    }
}