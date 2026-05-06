using System.Reflection;

namespace DynamicEvaluator.TypeSystem.Tests;

internal class UT_TypeFunctions
{
    private List<string> _singleParamFunctions;
    private List<string> _twoParamFunctions;
    private List<string> _multiParamFunctions;

    [OneTimeSetUp]
    public void Setup()
    {
        _singleParamFunctions = new List<string>();
        _twoParamFunctions = new List<string>();
        _multiParamFunctions = new List<string>();

        var methods = typeof(TypeFunctions).GetMethods(BindingFlags.Public | BindingFlags.Static);
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();

            bool isParams = parameters.Any(pi => pi.GetCustomAttribute<ParamArrayAttribute>() != null);
            if (isParams)
            {
                _multiParamFunctions.Add(method.Name);
            }
            else if (parameters.Length == 1)
            {
                _singleParamFunctions.Add(method.Name);
            }
            else if (parameters.Length == 2)
            {
                _twoParamFunctions.Add(method.Name);
            }
        }
    }

    public static IEnumerable<string> SingleParamNames
        => TypeFunctions.GetOneParamFunctions().Select(x => x.Method.Name);

    public static IEnumerable<string> TwoParamNames
        => TypeFunctions.GetTwoParamFunctions().Select(x => x.Method.Name);

    public static IEnumerable<string> MultiParamNames
        => TypeFunctions.GetVariableArgFunctions().Select(x => x.Method.Name);

    [TestCaseSource(nameof(SingleParamNames))]
    public void EnsureThat_All_SingleParamFunctionsArePublished(string functionName)
    {
        Assert.That(_singleParamFunctions, Contains.Item(functionName).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
    }

    [TestCaseSource(nameof(TwoParamNames))]
    public void EnsureThat_All_TwoParamFunctionsArePublished(string functionName)
    {
        Assert.That(_twoParamFunctions, Contains.Item(functionName).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
    }

    [TestCaseSource(nameof(MultiParamNames))]
    public void EnsureThat_All_MultiParamFunctionsArePublished(string functionName)
    {
        Assert.That(_multiParamFunctions, Contains.Item(functionName).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
    }
}
