﻿using System.Reflection;

namespace DynamicEvaluator.Tests;

[TestFixture]
public class DocumentationProviderTests
{
    private DocumentationProvider _documentationProvider;
    private HashSet<string> _skip;

    [OneTimeSetUp]
    public void Setup()
    {
        _documentationProvider = new DocumentationProvider();
        _skip =
        [
            "Pow"
        ];
    }

    public static IEnumerable<string> FunctionNames
    {
        get
        {
            var provider = new FunctionProvider();
            provider.FillFrom(typeof(Functions));
            return provider.GetFunctionNames();
        }
    }

    [TestCaseSource(nameof(FunctionNames))]
    public void EnsureThat_Function_IsDocumented(string function)
    {
        if (_skip.Contains(function))
        {
            Assert.Warn($"{function} is skipped from docu test");
            return;
        }

        string doc = _documentationProvider.GetDocumentation(function);

        Assert.Multiple(() =>
        {
            Assert.That(_documentationProvider.FunctionNames, Contains.Item(function).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
            Assert.That(doc, Has.Length.GreaterThan(0));
        });
    }
}
