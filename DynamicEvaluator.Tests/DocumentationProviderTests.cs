using DynamicEvaluator.Documentation;

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

        var doc = _documentationProvider.GetDocumentation(function);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_documentationProvider.FunctionNames, Contains.Item(function).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
            Assert.That(doc, Is.Not.Null);
            Assert.That(doc.Examples, Has.Length.GreaterThan(0));
            Assert.That(doc.Description, Is.Not.WhiteSpace);
        }
    }
}
