using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            "pow"
        ];
    }

    public static IEnumerable<string> FunctionNames
    {
        get
        {
            return typeof(Functions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(x => x.Name.ToLower());
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
