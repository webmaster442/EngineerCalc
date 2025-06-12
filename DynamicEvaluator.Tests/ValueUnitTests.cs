using DynamicEvaluator.Types;

namespace DynamicEvaluator.Tests;

[TestFixture]
internal class ValueUnitTests
{
    private ValueUnit V1 = new ValueUnit(10, "Meter");
    private ValueUnit V2 = new ValueUnit(10, "Foot");
    private ValueUnit V3 = new ValueUnit(2, "Second");

    [Test]
    public void Ensure_That_Operations_Work_ValueUnit_ValueUnit()
    {
        Assert.Multiple(() =>
        {
            Assert.That(V1 + V2, Is.EqualTo(new ValueUnit(13.04800, "Meter")));
            Assert.That(V1 - V2, Is.EqualTo(new ValueUnit(6.95200, "Meter")));
            Assert.That(V1 * V2, Is.EqualTo(new ValueUnit(30.48, "SquareMeter")));
            Assert.That(V1 / V2, Is.EqualTo(new ValueUnit(3.2808398950131233595800524934383, "")));
        });
    }

    [Test]
    public void Ensure_That_Operations_Work_ValueUnit_Double()
    {
        Assert.Multiple(() =>
        {
            Assert.That(V1 + 10, Is.EqualTo(new ValueUnit(20, "Meter")));
            Assert.That(V1 - 5, Is.EqualTo(new ValueUnit(5, "Meter")));
            Assert.That(V1 * 2, Is.EqualTo(new ValueUnit(20, "Meter")));
            Assert.That(V1 / 2, Is.EqualTo(new ValueUnit(5, "Meter")));
        });
    }
}
