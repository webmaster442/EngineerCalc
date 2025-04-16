using DynamicEvaluator.Types;

namespace DynamicEvaluator.Tests;

public class ExpressionTests
{
    private ExpressionFactory _expressionFactory;

    [SetUp]
    public void Setup()
    {
        _expressionFactory = new ExpressionFactory();
    }

    //Constants
    [TestCase("3", "0")]
    [TestCase("99", "0")]
    [TestCase("360", "0")]
    //1st order
    [TestCase("x", "1")]
    [TestCase("3x", "3")]
    [TestCase("3x+55", "3")]
    //2nd order
    [TestCase("x^2", "(2 * x)")]
    [TestCase("x^2+3x", "((2 * x) + 3)")]
    [TestCase("x^2+3x+22", "((2 * x) + 3)")]
    [TestCase("x^3", "(3 * (x ^ 2))")]
    //inverse
    [TestCase("1/x", "(-1 / (x ^ 2))")]
    //exponent
    [TestCase("2^x", "(0.6931471805599453 * (2 ^ x))")]
    //trigonometry
    [TestCase("sin(x)", "cos(x)")]
    [TestCase("cos(x)", "(-sin(x))")]
    [TestCase("tan(x)", "(cos(x) ^ -2)")]
    [TestCase("ctg(x)", "(-(sin(x) ^ -2))")]
    //Root
    [TestCase("root(x,2)", "(0.5 * (x ^ -0.5))")]
    //Logarithms
    [TestCase("ln(x)", "(1 / x)")]
    [TestCase("Log(x, e)", "(1 / x)")]
    [TestCase("Log(x, 4)", "(0.72134752044448171348 * (1 / x))")]
    public void EnsureThat_Differentiate_Works(string expression, string expected)
    {

        IExpression derived = _expressionFactory.Create(expression).Differentiate("x").Simplify();
        Assert.That(derived.ToString(), Is.EquivalentTo(expected));
    }

    [TestCase("1:2")]
    [TestCase("1Ö2")]
    [TestCase("1=2")]
    [TestCase("")]
    [TestCase("\t")]
    [TestCase(" ")]
    [TestCase("\r\n")]
    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("sin(99ö)")]
    [TestCase("root(99,)")]
    [TestCase("root(,99)")]
    public void EnsureThat_InvalidExpression_Throws(string expression)
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            IExpression parsed = _expressionFactory.Create(expression);
        });
    }

    [TestCase("1\t+1", 2)]
    [TestCase("1 +1", 2)]
    [TestCase("1\r+1", 2)]
    [TestCase("1\n+1", 2)]
    [TestCase("1\r\n+1", 2)]
    [TestCase("1+-2", -1)]
    [TestCase("x+y", 3)]
    public void EnsureThat_Evaluate_Works_Integers(string expression, long expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        Dictionary<string, dynamic> variables = new()
        {
            { "x", 1L },
            { "y", 2L },
        };
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<long>());
            Assert.That(result, Is.EqualTo(expected));
        });
    }

    [TestCase("22/11", 2, 1)]
    [TestCase("(1/2)*2+(4*3)", 13, 1)]
    [TestCase("x/y", 5, 1)]
    [TestCase("(1/2)*(3/4)", 3, 8)]
    public void EnsureThat_Evaluate_Works_Fractions(string expression, long numerator, long denominator)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        Dictionary<string, dynamic> variables = new()
        {
            { "x", 10L },
            { "y", 2L },
        };
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<Fraction>());
            Assert.That(result.Numerator, Is.EqualTo(numerator));
            Assert.That(result.Denominator, Is.EqualTo(denominator));
        });
    }
}
