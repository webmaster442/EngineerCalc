﻿namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class DegExpression : RewritableFunctionExpression
{
    public DegExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new DivideExpression(new MultiplyExpression(original, new ConstantExpression(180L)), new VariableExpression("pi"));
    }
}
