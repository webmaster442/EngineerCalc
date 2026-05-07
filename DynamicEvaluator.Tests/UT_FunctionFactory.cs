using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

internal class UT_FunctionFactory
{
    private List<string> _typeFunctionNames;

    [OneTimeSetUp]
    public void Setup()
    {
        _typeFunctionNames = typeof(TypeFunctions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Select(x => x.Name)
            .ToList();
    }

    public static IEnumerable<string> FunctionFactoryNames
    {
        get
        {
            return new FunctionFactory();
        }
    }

    [TestCaseSource(nameof(FunctionFactoryNames))]

    public void EnsureThat_FunctionFactory_HasAllTypeFunctions(string functionName)
    {
        Assert.That(_typeFunctionNames, Contains.Item(functionName));
    }
}
