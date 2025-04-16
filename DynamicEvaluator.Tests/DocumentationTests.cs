using System.Reflection;

namespace DynamicEvaluator.Tests;

internal class DocumentationTests
{
    private HashSet<string> _documented;
    private HashSet<string> _skip;

    [OneTimeSetUp]
    public void Setup()
    {
       _documented = typeof(Functions).Assembly.GetManifestResourceNames()
            .Select(x => x.Replace("DynamicEvaluator.Docs.", "").ToLower())
            .ToHashSet();

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
    public void TestFunctionsToBeDocumented(string functionName)
    {
        if (_skip.Contains(functionName))
        {
            Assert.Warn($"{functionName} is skipped from docu test");
            return;
        }

        var docuName = $"{functionName}.md";
        Assert.That(_documented, Contains.Item(docuName));
    }
}
