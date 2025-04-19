using DynamicEvaluator;

namespace EngineerCalc.Calculator;

public class State
{
    public VariablesAndConstantsCollection Variables { get; }

    public State()
    {
        Variables = new VariablesAndConstantsCollection();
    }
}
