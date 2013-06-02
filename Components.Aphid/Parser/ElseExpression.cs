using System;
using System.Collections.Generic;

namespace Components.Aphid.Parser
{
    public class ElseExpression
    {
        public List<Expression> Body { get; set; }

        public ElseExpression()
        {
        }

        public ElseExpression(List<Expression> body)
        {
            Body = body;
        }
    }
}

