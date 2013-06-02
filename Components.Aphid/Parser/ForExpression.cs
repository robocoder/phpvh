using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class ForExpression : Expression
    {
        public Expression Initialization { get; set; }

        public Expression Condition { get; set; }

        public Expression Afterthought { get; set; }

        public List<Expression> Body { get; set; }

        public ForExpression(
            Expression initialization,
            Expression condition,
            Expression afterthought,
            List<Expression> body)
        {
            Initialization = initialization;
            Condition = condition;
            Afterthought = afterthought;
            Body = body;
        }
    }
}
