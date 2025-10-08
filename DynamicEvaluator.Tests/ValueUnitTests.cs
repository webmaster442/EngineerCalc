using DynamicEvaluator.Types;

namespace DynamicEvaluator.Tests;

[TestFixture]
internal class ValueUnitTests
{
    private readonly ValueUnit _v1 = new ValueUnit(10, "Meter");
    private readonly ValueUnit _v2 = new ValueUnit(10, "Foot");

    [Test]
    public void Ensure_That_Operations_Work_ValueUnit_ValueUnit()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_v1 + _v2, Is.EqualTo(new ValueUnit(13.04800, "Meter")));
            Assert.That(_v1 - _v2, Is.EqualTo(new ValueUnit(6.95200, "Meter")));
            Assert.That(_v1 * _v2, Is.EqualTo(new ValueUnit(30.48, "SquareMeter")));
            Assert.That(_v1 / _v2, Is.EqualTo(new ValueUnit(3.2808398950131233595800524934383, "")));
        }
    }

    [Test]
    public void Ensure_That_Operations_Work_ValueUnit_Double()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_v1 + 10, Is.EqualTo(new ValueUnit(20, "Meter")));
            Assert.That(_v1 - 5, Is.EqualTo(new ValueUnit(5, "Meter")));
            Assert.That(_v1 * 2, Is.EqualTo(new ValueUnit(20, "Meter")));
            Assert.That(_v1 / 2, Is.EqualTo(new ValueUnit(5, "Meter")));
        }
    }
}
