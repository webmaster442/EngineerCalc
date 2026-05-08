//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.Documentation;

namespace DynamicEvaluator.Tests;

[TestFixture]
public class UT_DocumentationProvider
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
        => new FunctionFactory();

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
            Assert.That(doc.Summary, Is.Not.WhiteSpace);
        }
    }


    public static IEnumerable<string> DocumentationFunctionNames
    {
        get
        {
            return new DocumentationProvider().Select(m => m.Name);
        }
    }


    [TestCaseSource(nameof(DocumentationFunctionNames))]
    public void EnsureThat_Function_InDoc_Exists_InCode(string documentedName)
    {
        Assert.That(new FunctionFactory(), Contains.Item(documentedName).Using((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
    }
}
