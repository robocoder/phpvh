using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class ArrayExpression : Expression
    {
        public List<Expression> Elements { get; set; }
    }
}
