using EngineerCalc.Extensions;

namespace EngineerCalc.Tests;

[TestFixture]
public class StringExtensionTests
{
    [TestCase("", 0, 0)]
    [TestCase("foo bar baz", 0, 0)]
    [TestCase("foo bar baz", 1, 0)]
    [TestCase("foo bar baz", 2, 0)]
    [TestCase("foo bar baz", 3, 0)]
    [TestCase("foo bar baz", 4, 1)]
    [TestCase("foo bar baz", 5, 1)]
    [TestCase("foo bar baz", 6, 1)]
    [TestCase("foo bar baz", 7, 1)]
    [TestCase("foo bar baz", 8, 2)]
    [TestCase("foo bar baz", 9, 2)]
    [TestCase("foo bar baz", 10, 2)]
    [TestCase("foo bar baz", 111, 2)]
    public void ToWordIndexTest(string input, int position, int expected)
    {
        int returnValue = input.ToWordIndex(position);
        Assert.That(returnValue, Is.EqualTo(expected), $"Expected ToWordIndex('{input}', {position}) to return {expected}, but got {returnValue}.");
    }
}
