//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Tests;

[TestFixture]
public class UT_FunctionExpressons
{
    private ExpressionFactory _factory;
    private VariablesAndConstantsCollection _variables;

    [SetUp]
    public void Setup()
    {
        _factory = new ExpressionFactory();
        _variables = new VariablesAndConstantsCollection();
    }

    [TestCase("abs(-1)", "1", TypeState.Integer)]
    [TestCase("abs(-1.25)", "1.25", TypeState.Double)]
    [TestCase("abs(-3/4)", "3 / 4", TypeState.Fraction)]
    [TestCase("abs(cplx(-4,3))", "5", TypeState.Integer)]
    [TestCase("ln(e)", "1", TypeState.Integer)]
    [TestCase("ln(2)", "0.6931471805599453", TypeState.Double)]
    [TestCase("ln(1 / 4)", "-1.3862943611198906", TypeState.Double)]
    [TestCase("ln(cplx(1,1))", "<0.3465735902799727; 0.7853981633974483>", TypeState.Complex)]
    [TestCase("log(e, e)", "1", TypeState.Integer)]
    [TestCase("log(2, e)", "0.6931471805599453", TypeState.Double)]
    [TestCase("log(1 / 4, e)", "-1.3862943611198906", TypeState.Double)]
    [TestCase("log(cplx(1,1), e)", "<0.3465735902799727; 0.7853981633974483>", TypeState.Complex)]
    [TestCase("pow(2, 10)", "1024", TypeState.Integer)]
    [TestCase("pow(2.2, 11)", "5843.183014113285", TypeState.Double)]
    [TestCase("pow(1 / 4, 2)", "0.0625", TypeState.Double)]
    [TestCase("pow(cplx(1,1), 2)", "<1.2246467991473535E-16; 2.0000000000000004>", TypeState.Complex)]
    [TestCase("root(2, 2)", "1.4142135623730951", TypeState.Double)]
    [TestCase("root(1 / 4, 2)", "0.5", TypeState.Double)]
    [TestCase("root(4, 2)", "2", TypeState.Integer)]
    [TestCase("root(cplx(1,1), 2)", "<1.0986841134678098; 0.45508986056222733>", TypeState.Complex)]
    [TestCase("sqrt(2)", "1.4142135623730951", TypeState.Double)]
    [TestCase("sqrt(1 / 4)", "0.5", TypeState.Double)]
    [TestCase("sqrt(4)", "2", TypeState.Integer)]
    [TestCase("sqrt(cplx(1,1))", "<1.09868411346781; 0.45508986056222733>", TypeState.Complex)]
    [TestCase("gcd(15, 5)", "5", TypeState.Integer)]
    [TestCase("lcm(15, 4)", "60", TypeState.Integer)]
    [TestCase("factorial(5)", "120", TypeState.Integer)]
    [TestCase("not(1)", "-2", TypeState.Integer)]
    [TestCase("and(127, 255)", "127", TypeState.Integer)]
    [TestCase("or(127, 255)", "255", TypeState.Integer)]
    [TestCase("xor(127, 127)", "0", TypeState.Integer)]
    [TestCase("shiftleft(1, 2)", "4", TypeState.Integer)]
    [TestCase("shiftright(4, 2)", "1", TypeState.Integer)]
    [TestCase("binomial(5, 2)", "10", TypeState.Integer)]
    [TestCase("floor(1.25)", "1", TypeState.Integer)]
    [TestCase("floor(5 / 4)", "1", TypeState.Integer)]
    [TestCase("ceiling(1.25)", "2", TypeState.Integer)]
    [TestCase("ceiling(5 / 4)", "2", TypeState.Integer)]
    [TestCase("sin(0)", "0", TypeState.Integer)]
    [TestCase("sin(1.1)", "0.8912073600614354", TypeState.Double)]
    [TestCase("sin(1 / 4)", "0.24740395925452294", TypeState.Double)]
    [TestCase("sin(cplx(1, 1))", "<1.2984575814159773; 0.6349639147847361>", TypeState.Complex)]
    [TestCase("cos(0)", "1", TypeState.Integer)]
    [TestCase("cos(1.1)", "0.4535961214255773", TypeState.Double)]
    [TestCase("cos(1 / 4)", "0.9689124217106447", TypeState.Double)]
    [TestCase("cos(cplx(1, 1))", "<0.8337300251311491; -0.9888977057628651>", TypeState.Complex)]
    [TestCase("tan(0)", "0", TypeState.Integer)]
    [TestCase("tan(1.1)", "1.9647596572486523", TypeState.Double)]
    [TestCase("arcsin(0)", "0", TypeState.Integer)]
    [TestCase("arcsin(0.8912073600614354)", "1.1", TypeState.Double)]
    [TestCase("arcsin(0.24740395925452294)", "0.25", TypeState.Double)]
    [TestCase("arcsin(cplx(1, 1))", "<0.6662394324925152; 1.0612750619050357>", TypeState.Complex)]
    [TestCase("arccos(1)", "0", TypeState.Integer)]
    [TestCase("arccos(0.4535961214255773)", "1.1", TypeState.Double)]
    [TestCase("arccos(0.9689124217106447)", "0.2500000000000002", TypeState.Double)]
    [TestCase("arccos(cplx(1, 1))", "<0.9045568943023814; -1.0612750619050357>", TypeState.Complex)]
    [TestCase("fromhex('ff')", "255", TypeState.Integer)]
    [TestCase("fromhexsigned('ff')", "-1", TypeState.Integer)]
    [TestCase("frombin('1010')", "10", TypeState.Integer)]
    [TestCase("frombinsigned('1010')", "-6", TypeState.Integer)]
    [TestCase("tohex(255)", "FF", TypeState.String)]
    [TestCase("tohex(-255)", "F01", TypeState.String)]
    [TestCase("tobin(10)", "1010", TypeState.String)]
    [TestCase("tobin(-10)", "10110", TypeState.String)]
    [TestCase("min(1, 2, 3)", "1", TypeState.Integer)]
    [TestCase("max(1, 2, 3)", "3", TypeState.Integer)]
    [TestCase("average(1, 2, 3)", "2", TypeState.Integer)]
    [TestCase("sum(1, 2, 3)", "6", TypeState.Integer)]
    [TestCase("count(1, 2, 3)", "3", TypeState.Integer)]
    [TestCase("min(array(1, 2, 3))", "1", TypeState.Integer)]
    [TestCase("max(array(1, 2, 3))", "3", TypeState.Integer)]
    [TestCase("average(array(1, 2, 3))", "2", TypeState.Integer)]
    [TestCase("sum(array(1, 2, 3))", "6", TypeState.Integer)]
    [TestCase("count(array(1, 2, 3))", "3", TypeState.Integer)]
    public void EnsureThat_Function_Evaluated_ReturnsExpectedValue(string expression,
                                                                   string expected,
                                                                   TypeState expectedState)
    {
        var expressionTree = _factory.Create(expression);
        Result result = expressionTree.Evaluate(_variables);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.TypeState, Is.EqualTo(expectedState));
            Assert.That(result.ToString(), Is.EqualTo(expected));
        }
    }
}
