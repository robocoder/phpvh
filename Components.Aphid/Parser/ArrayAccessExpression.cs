using System;

namespace Components.Aphid.Parser
{
    public class ArrayAccessExpression : Expression
    {
        public Expression ArrayExpression { get; set; }

        public Expression KeyExpression { get; set; }

        public ArrayAccessExpression ()
        {
        }

        public ArrayAccessExpression(Expression arrayExpression, Expression keyExpression)
        {
            ArrayExpression = arrayExpression;
            KeyExpression = keyExpression;
        }
    }
}

