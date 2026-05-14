namespace DynamicEvaluator.Tests;

[TestFixture]
internal class UT_UnitConverter
{
    UnitConverter _sut;

    [OneTimeSetUp]
    public void Setup()
    {
        _sut = new UnitConverter();
    }

    [TestCase("kg", "g", 1, 1000)]
    [TestCase("kg", "g", 0.001, 1)]
    [TestCase("dkg", "g", 1, 10)]
    [TestCase("dkg", "g", 0.1, 1)]
    [TestCase("t", "kg", 1, 1000)]
    [TestCase("mg", "g", 1000, 1)]
    public void EnsureThat_Convert_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
    {
        double result = _sut.Convert(fromUnit, toUnit, value);
        Assert.That(result, Is.EqualTo(expected).Within(0.0001));
    }
}
