using System.Numerics;

using DynamicEvaluator.Types;

namespace DynamicEvaluator.Tests;

[TestFixture]
public class SerializerTests
{
    private VariablesAndConstantsCollection _collection;

    [SetUp]
    public void Setup()
    {
        _collection = new VariablesAndConstantsCollection
        {
            { "vect", new Vector2(22, 13) },
            { "x", 11f },
            { "z", 11d },
            { "comp", new Complex(0, 1) },
            { "f", new Fraction(1, 3) }
        };
    }

    [Test]
    public void Ensure_That_Serializer_ToJson_ProducesResult()
    {
        var json = _collection.ToJson();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.Not.Empty);
        }
    }

    [Test]
    public void Ensure_That_Serializer_ToJson_FromJson_RoundtripWorks()
    {
        var json = _collection.ToJson();
        var copy = new VariablesAndConstantsCollection();
        copy.FromJson(json, true);

        Assert.That(copy.Variables(), Is.EquivalentTo(_collection.Variables()));
    }
}
