using System;
using System.Collections.Generic;

namespace Components.Aphid.Parser
{
    public class ForEachExpression : Expression
    {
        public Expression Collection { get; set; }

        public Expression Element { get; set; }

        public List<Expression> Body { get; set; }

        public ForEachExpression (Expression collection, Expression element, List<Expression> body)
        {
            Collection = collection;
            Element = element;
            Body = body;
        }
    }
}

