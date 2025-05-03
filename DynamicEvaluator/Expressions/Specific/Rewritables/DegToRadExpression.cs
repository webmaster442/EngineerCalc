﻿namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class DegToRadExpression : RewritableFunctionExpression
{
    public DegToRadExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new MultiplyExpression(new DivideExpression(original, new ConstantExpression(180L)), new VariableExpression("pi"));
    }
}
