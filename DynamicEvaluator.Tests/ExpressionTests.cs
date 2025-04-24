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
    [TestCase("root(x,2)", "(1 / 2 * (x ^ -1 / 2))")]
    //Logarithms
    [TestCase("ln(x)", "(1 / x)")]
    [TestCase("Log(x, e)", "(1 / x)")]
    [TestCase("Log(x, 4)", "(0.7213475204444817 * (1 / x))")]
    public void EnsureThat_Differentiate_Works(string expression, string expected)
    {
        IExpression derived = _expressionFactory.Create(expression).Differentiate("x").Simplify();
        Assert.That(derived.ToString(), Is.EquivalentTo(expected));
    }

    //add
    [TestCase("1+1", "2")]
    [TestCase("0+y", "y")]
    [TestCase("y+0", "y")]
    [TestCase("x + -y", "(x - y)")]
    [TestCase("-x + -y", "((-x) - y)")]
    [TestCase("-x + y", "(y - x)")]
    //and
    [TestCase("true & true", "True")]
    [TestCase("true & false",  "False")]
    [TestCase("false & true", "False")]
    [TestCase("false & false", "False")]
    [TestCase("true & x", "x")]
    [TestCase("x & true", "x")]
    [TestCase("false & x", "False")]
    [TestCase("x & false", "False")]
    [TestCase("x & x", "x")]
    //or
    [TestCase("true | true", "True")]
    [TestCase("true | false", "True")]
    [TestCase("false | true", "True")]
    [TestCase("false | false", "False")]
    [TestCase("true | x", "True")]
    [TestCase("x | true", "True")]
    [TestCase("false | x", "x")]
    [TestCase("x | false", "x")]
    [TestCase("x | x", "x")]
    //constant
    [TestCase("1", "1")]
    [TestCase("0.5", "0.5")]
    [TestCase("true", "True")]
    //divide
    [TestCase("1 / 0.1", "10")]
    [TestCase("1/2", "1 / 2")]
    [TestCase("1/1", "1")]
    [TestCase("y/x", "(y / x)")]
    [TestCase("x/1", "x")]
    [TestCase("x/-1", "(-x)")]
    [TestCase("-x / -y", "(x / y)")]
    //exponent
    [TestCase("x^0", "1")]
    [TestCase("0.14^0", "1")]
    [TestCase("x^1", "x")]
    [TestCase("3^1", "3")]
    [TestCase("0^y", "0")]
    [TestCase("x ^ y", "(x ^ y)")]
    //ln
    [TestCase("ln(e)", "1")]
    //log
    [TestCase("log(2, 2)", "1")]
    //negate
    [TestCase("!true", "False")]
    [TestCase("!false", "True")]
    [TestCase("!x", "(!x)")]
    [TestCase("!(!x)", "x")]
    [TestCase("!(!true)", "True")]
    [TestCase("!(!false)", "False")]
    [TestCase("!(x & y)", "(x | y)")]
    [TestCase("!(x | y)", "(x & y)")]
    //Multiply
    [TestCase("0*y", "0")]
    [TestCase("y*0", "0")]
    [TestCase("1*y", "y")]
    [TestCase("y*1", "y")]
    [TestCase("-1*y", "(-y)")]
    [TestCase("-1*-y", "y")]
    [TestCase("y*-1", "(-y)")]
    [TestCase("-y*-1", "y")]
    [TestCase("-y*-x", "(y * x)")]
    [TestCase("x*y", "(x * y)")]
    //Subtrasct
    [TestCase("0-y", "(-y)")]
    [TestCase("0--y", "y")]
    [TestCase("x-0", "x")]
    [TestCase("x--y", "(x + y)")]
    [TestCase("x-y", "(x - y)")]
    //Root
    [TestCase("root(2, 2)", "1")]
    [TestCase("root(x, 0)", "1")]
    [TestCase("root(x, 1)", "x")]
    [TestCase("root(x, y)", "(x ^ (1 / y))")]
    //sin
    [TestCase("sin(pi)", "0")]
    [TestCase("sin(2*pi)", "0")]
    [TestCase("sin(8*pi)", "0")]
    [TestCase("sin(0)", "0")]
    [TestCase("sin(x)", "sin(x)")]
    //cos
    [TestCase("cos(pi)", "-1")]
    [TestCase("cos(2*pi)", "-1")]
    [TestCase("cos(8*pi)", "-1")]
    [TestCase("cos(0)", "1")]
    [TestCase("cos(x)", "cos(x)")]
    //tan
    [TestCase("tan(pi)", "0")]
    [TestCase("tan(2*pi)", "0")]
    [TestCase("tan(8*pi)", "0")]
    [TestCase("tan(0)", "0")]
    [TestCase("tan(x)", "tan(x)")]
    //ctg
    [TestCase("ctg(pi)", "Infinity")]
    [TestCase("ctg(2*pi)", "Infinity")]
    [TestCase("ctg(8*pi)", "Infinity")]
    [TestCase("ctg(0)", "Infinity")]
    [TestCase("ctg(x)", "ctg(x)")]
    public void EnsureThat_Simplify_Works(string expression, string expected)
    {
        IExpression simplified = _expressionFactory.Create(expression).Simplify();
        Assert.That(simplified.ToString(), Is.EqualTo(expected));
    }

    [TestCase("false", "False")]
    [TestCase("true", "True")]
    [TestCase("!a&!b|!a&b", "(!a)")]
    [TestCase("a&!b | a&b", "a")]
    [TestCase("!a&b | a&!b", "((b & (!a)) | ((!b) & a))")]
    [TestCase("!a&!b | !a&b | a&!b | a&b", "True")]
    [TestCase("d&(c&b|b&d)|!(d&a)", "((b | (!d)) | (!a))")]
    [TestCase("!a&!b&!c&!d|!a&!b&!c&d|!a&b&!c&!d|!a&b&!c&d|a&b&!c&!d|a&b&!c&d", "(((!c) & b) | ((!c) & (!a)))")]
    public void EnsureThat_LogicSimplify_Works(string expression, string expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        bool result = parsed.TrySimplfyAsLogicExpression(out var simplified);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(simplified?.ToString(), Is.EqualTo(expected));
        });
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
    [TestCase("33-22", 11)]
    [TestCase("33+22", 55)]
    [TestCase("x+y", 3)]
    [TestCase("fromhex('ff')", 255)]
    [TestCase("1_000+100", "1100")]
    public void EnsureThat_Evaluate_Works_Integers(string expression, long expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        VariablesAndConstantsCollection variables = new()
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

    [TestCase("1.1", 1.1d)]
    [TestCase("1e-3", 0.001d)]
    [TestCase("1.1e3", 1100d)]
    [TestCase("1.1e+3", 1100d)]
    [TestCase("sin(0)", 0d)]
    [TestCase("cos(0)", 1d)]
    [TestCase("tan(0)", 0d)]
    [TestCase("tan(x)", 1.5574077246549023d)]
    [TestCase("tan(y)", -2.1850398632615189d)]
    [TestCase("ctg(x)", 0.64209261593433065d)]
    [TestCase("ln(e)", 1)]
    [TestCase("ln(100)", 4.6051701859880913680359829093687)]
    [TestCase("log(1024,2)", 10)]
    [TestCase("1_0+1.1", 11.1)]
    [TestCase("sin(pi/2)", 1.0d)]
    [TestCase("2*kilo", 2000.0d)]
    [TestCase("21*milli", 0.021d)]
    public void EnsureThat_Evaluate_Works_Doubles(string expression, double expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        VariablesAndConstantsCollection variables = new()
        {
            { "x", 1d },
            { "y", 2d },
        };
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<double>());
            Assert.That(result, Is.EqualTo(expected).Within(0.001));
        });
    }

    [TestCase("'foo'", "foo")]
    [TestCase("'foo'+'foo'", "foofoo")]
    [TestCase("\"foo\"", "foo")]
    [TestCase("\"foo\"+\"foo\"", "foofoo")]
    public void EnsureThat_Evaluate_Works_Strings(string expression, string expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        VariablesAndConstantsCollection variables = new()
        {
            { "foo", "foo" },
        };
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<string>());
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
        VariablesAndConstantsCollection variables = new()
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

    [TestCase("false&false", false)]
    [TestCase("false&true", false)]
    [TestCase("true&false", false)]
    [TestCase("true&true", true)]
    [TestCase("false|false", false)]
    [TestCase("false|true", true)]
    [TestCase("true|false", true)]
    [TestCase("true|true", true)]
    [TestCase("!false", true)]
    [TestCase("!true", false)]
    [TestCase("!(!true)", true)]
    [TestCase("!false&!false", true)]
    [TestCase("false", false)]
    [TestCase("true", true)]
    public void EnsureThat_Evaluate_Works_Logics(string expression, bool expected)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        VariablesAndConstantsCollection variables = new();
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<bool>());
            Assert.That(result, Is.EqualTo(expected));
        });
    }

    [TestCase("random()", 0, long.MaxValue)]
    [TestCase("random(1, 5)", 1, 5)]
    public void EnsureThat_FunctionOverload_Works_ForRandom(string expression, long minValue, long maxValue)
    {
        IExpression parsed = _expressionFactory.Create(expression);
        VariablesAndConstantsCollection variables = new();
        dynamic result = parsed.Evaluate(variables);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<long>());
            Assert.That(result, Is.GreaterThanOrEqualTo(minValue));
            Assert.That(result, Is.LessThan(maxValue));
        });
    }
}
