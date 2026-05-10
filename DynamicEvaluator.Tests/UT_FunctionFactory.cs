//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

internal class UT_FunctionFactory
{
    private string[] _functionFactoryNames;

    [OneTimeSetUp]
    public void Setup()
    {
        _functionFactoryNames = new FunctionFactory().ToArray();
    }

    public static IEnumerable<string> TypeFunctionNames
    {
        get
        {
            return typeof(TypeFunctions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(x => x.Name)
                .ToList();
        }
    }


    [TestCaseSource(nameof(TypeFunctionNames))]

    public void EnsureThat_FunctionFactory_HasAllTypeFunctions(string functionName)
    {
        Assert.That(_functionFactoryNames, Contains.Item(functionName));
    }
}
