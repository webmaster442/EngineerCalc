using System.Numerics;

using DynamicEvaluator.TypeSystem.InternalTypes;

using NUnit.Framework.Internal;

namespace DynamicEvaluator.TypeSystem.Tests;


[TestFixture]
public class UT_Result
{
    [Test]
    public void EnsureThat_FromDouble_ReturnsInteger_WhenValueIsWholeNumber()
    {
        // Arrange
        double value = 42.0;
        // Act
        var result = Result.FromDouble(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Integer));
        Assert.That(result.CastToBigInteger(), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void EnsureThat_FromDouble_ReturnsDouble_WhenValueIsNotWholeNumber()
    {
        // Arrange
        double value = 3.14;
        // Act
        var result = Result.FromDouble(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Double));
        Assert.That(result.CastToDouble(), Is.EqualTo(3.14));
    }

    public void EnsureThat_FromComplex_ReturnsInteger_WhenValueIsWholeNumber()
    {
        // Arrange
        Complex value = new Complex(42.0, 0);
        // Act
        var result = Result.FromComplex(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Integer));
        Assert.That(result.CastToBigInteger(), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void EnsureThat_FromComplex_ReturnsComplex_WhenValueIsNotWholeNumber()
    {
        // Arrange
        Complex value = new Complex(3.14, 2.71);
        // Act
        var result = Result.FromComplex(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Complex));
        Assert.That(result.CastToComplex(), Is.EqualTo(value));
    }

    [Test]
    public void EnsureThat_FromFraction_ReturnsInteger_WhenValueIsWholeNumber()
    {
        // Arrange
        Fraction value = new Fraction(42, 1);
        // Act
        var result = Result.FromFraction(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Integer));
        Assert.That(result.CastToBigInteger(), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void EnsureThat_FromFraction_ReturnsFraction_WhenValueIsNotWholeNumber()
    {
        // Arrange
        Fraction value = new Fraction(1, 3);
        // Act
        var result = Result.FromFraction(value);
        // Assert
        Assert.That(result.TypeState, Is.EqualTo(TypeState.Fraction));
        Assert.That(result.CastToFraction, Is.EqualTo(value));
    }
}
