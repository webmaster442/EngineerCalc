using System.Globalization;

namespace DynamicEvaluator.Tests;

[TestFixture]
internal class StringifyTests
{
    [TestCase("str", "str")]
    [TestCase(1.11, "1.11\t\n{{ 1.11 }}")]
    [TestCase(10000L, "10,000\t\n{{ 10,000 }}")]
    [TestCase(10000.11, "10,000.11\t\n{{ 10,000.11 }}")]
    public void EnsureThat_Stringify_Works(object input, string expected)
    {
        string actual = input.FormatToLatex(new CultureInfo("En-us"));
        Assert.That(actual, Is.EqualTo(expected));
    }
}
