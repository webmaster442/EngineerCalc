namespace DynamicEvaluator.TypeSystem.Tests;

public class UT_TypeHelper
{
    [TestCase(TypeState.NoResult, TypeState.Integer)]
    [TestCase(TypeState.NoResult, TypeState.Double)]
    [TestCase(TypeState.NoResult, TypeState.Complex)]
    [TestCase(TypeState.NoResult, TypeState.Fraction)]
    [TestCase(TypeState.NoResult, TypeState.String)]
    [TestCase(TypeState.NoResult, TypeState.Boolean)]
    [TestCase(TypeState.NoResult, TypeState.Array)]
    [TestCase(TypeState.String, TypeState.NoResult)]
    [TestCase(TypeState.String, TypeState.Integer)]
    [TestCase(TypeState.String, TypeState.Double)]
    [TestCase(TypeState.String, TypeState.Complex)]
    [TestCase(TypeState.String, TypeState.Fraction)]
    [TestCase(TypeState.String, TypeState.String)]
    [TestCase(TypeState.String, TypeState.Boolean)]
    [TestCase(TypeState.String, TypeState.Array)]
    [TestCase(TypeState.Array, TypeState.NoResult)]
    [TestCase(TypeState.Array, TypeState.Integer)]
    [TestCase(TypeState.Array, TypeState.Double)]
    [TestCase(TypeState.Array, TypeState.Complex)]
    [TestCase(TypeState.Array, TypeState.Fraction)]
    [TestCase(TypeState.Array, TypeState.String)]
    [TestCase(TypeState.Array, TypeState.Boolean)]
    [TestCase(TypeState.Array, TypeState.Array)]
    [TestCase(TypeState.Integer, TypeState.NoResult)]
    [TestCase(TypeState.Double, TypeState.NoResult)]
    [TestCase(TypeState.Complex, TypeState.NoResult)]
    [TestCase(TypeState.Fraction, TypeState.NoResult)]
    [TestCase(TypeState.String, TypeState.NoResult)]
    [TestCase(TypeState.Boolean, TypeState.NoResult)]
    [TestCase(TypeState.Array, TypeState.NoResult)]
    [TestCase(TypeState.NoResult, TypeState.Array)]
    [TestCase(TypeState.Integer, TypeState.Array)]
    [TestCase(TypeState.Double, TypeState.Array)]
    [TestCase(TypeState.Complex, TypeState.Array)]
    [TestCase(TypeState.Fraction, TypeState.Array)]
    [TestCase(TypeState.String, TypeState.Array)]
    [TestCase(TypeState.Boolean, TypeState.Array)]
    [TestCase(TypeState.Array, TypeState.Array)]
    [TestCase(TypeState.NoResult, TypeState.String)]
    [TestCase(TypeState.Integer, TypeState.String)]
    [TestCase(TypeState.Double, TypeState.String)]
    [TestCase(TypeState.Complex, TypeState.String)]
    [TestCase(TypeState.Fraction, TypeState.String)]
    [TestCase(TypeState.String, TypeState.String)]
    [TestCase(TypeState.Boolean, TypeState.String)]
    [TestCase(TypeState.Array, TypeState.String)]

    public void EnsureThat_GetResultTypeState_ThrowsIncomptable(TypeState left, TypeState right)
    {
        Assert.Throws<TypeException>(() => Internals.TypeHelper.GetResultTypeState(left, right));
    }

    [TestCase(TypeState.NoResult)]
    [TestCase(TypeState.Integer)]
    [TestCase(TypeState.Double)]
    [TestCase(TypeState.Boolean)]
    [TestCase(TypeState.Complex)]
    [TestCase(TypeState.Fraction)]
    public void EnsureThat_GetResultTypeState_ReturnsLeft_WhenBothAreSame(TypeState left)
    {
        var result = Internals.TypeHelper.GetResultTypeState(left, left);
        Assert.That(result, Is.EqualTo(left));
    }
}
