using DynamicEvaluator.Types;

namespace DynamicEvaluator.Tests;

[TestFixture]
internal class FractionTests
{
    [TestCase(2, 6, 1, 4, 7, 12)]
    [TestCase(3, 10, 1, 5, 1, 2)]
    [TestCase(5, 16, 5, 12, 35, 48)]
    [TestCase(3, 8, 9, 16, 15, 16)]
    public void EnsureThat_Operator_Add_Works(long n1, long d1, long n2, long d2, long expN, long expD)
    {
        Fraction fract1 = new Fraction(n1, d1);
        Fraction fract2 = new Fraction(n2, d2);

        Fraction actual = fract1 + fract2;

        Assert.That(actual, Is.EqualTo(new Fraction(expN, expD)));
    }

    [TestCase(2, 6, 1, 4, 1, 12)]
    [TestCase(3, 10, 1, 5, 1, 10)]
    [TestCase(5, 16, 5, 12, -5, 48)]
    [TestCase(3, 8, 9, 16, -3, 16)]
    public void EnsureThat_Operator_Subtract_Works(long n1, long d1, long n2, long d2, long expN, long expD)
    {
        Fraction fract1 = new Fraction(n1, d1);
        Fraction fract2 = new Fraction(n2, d2);

        Fraction actual = fract1 - fract2;

        Assert.That(actual, Is.EqualTo(new Fraction(expN, expD)));
    }

    [TestCase(2, 6, 1, 4, 1, 12)]
    [TestCase(3, 10, 1, 5, 3, 50)]
    [TestCase(5, 16, 5, 12, 25, 192)]
    [TestCase(3, 8, 9, 16, 27, 128)]
    public void EnsureThat_Operator_Multiply_Works(long n1, long d1, long n2, long d2, long expN, long expD)
    {
        Fraction fract1 = new Fraction(n1, d1);
        Fraction fract2 = new Fraction(n2, d2);

        Fraction actual = fract1 * fract2;

        Assert.That(actual, Is.EqualTo(new Fraction(expN, expD)));
    }

    [TestCase(2, 6, 1, 4, 4, 3)]
    [TestCase(3, 10, 1, 5, 3, 2)]
    [TestCase(5, 16, 5, 12, 3, 4)]
    [TestCase(3, 8, 9, 16, 2, 3)]
    public void EnsureThat_Operator_Divide_Works(long n1, long d1, long n2, long d2, long expN, long expD)
    {
        Fraction fract1 = new Fraction(n1, d1);
        Fraction fract2 = new Fraction(n2, d2);

        Fraction actual = fract1 / fract2;

        Assert.That(actual, Is.EqualTo(new Fraction(expN, expD)));
    }

    [Test]
    public void EnsureThat_Operator_Increment_Wokrs()
    {
        Fraction fraction = new Fraction(3, 2);
        Assert.Multiple(() =>
        {
            Assert.That(++fraction, Is.EqualTo(new Fraction(5, 2)));
            Assert.That(fraction++, Is.EqualTo(new Fraction(5, 2)));
        });
    }

    [Test]
    public void EnsureThat_Operator_Decrement_Wokrs()
    {
        Fraction fraction = new Fraction(3, 2);
        Assert.Multiple(() =>
        {
            Assert.That(--fraction, Is.EqualTo(new Fraction(1, 2)));
            Assert.That(fraction--, Is.EqualTo(new Fraction(1, 2)));
        });
    }


    [Test]
    public void EnsureThat_Operator_UnaryPlus_Minus_Wokrs()
    {
        Fraction fraction1 = new Fraction(3, 2);
        Fraction fraction2 = new Fraction(-3, 2);
        Assert.Multiple(() =>
        {
            Assert.That(-fraction1, Is.EqualTo(new Fraction(-3, 2)));
            Assert.That(+fraction2, Is.EqualTo(new Fraction(-3, 2)));

        });
    }

    [Test]
    public void EnsureThat_Operator_Comparisuons_Wokrs()
    {
        Fraction fraction1 = new Fraction(1, 2);
        Fraction fraction2 = new Fraction(3, 2);
        Assert.Multiple(() =>
        {
            Assert.That(fraction1 < fraction2, Is.EqualTo(true));
            Assert.That(fraction2 > fraction1, Is.EqualTo(true));

        });
    }
}
