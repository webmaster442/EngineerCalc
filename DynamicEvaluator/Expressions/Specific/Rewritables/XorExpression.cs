using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class XorExpression : RewritableExpression
{
    public XorExpression(IExpression left, IExpression right)
    {
        _rewritten = new OrExpression(
            new AndExpression(left, new LogicNegateExpression(right)),
            new AndExpression(new LogicNegateExpression(left), right));
    }
}
