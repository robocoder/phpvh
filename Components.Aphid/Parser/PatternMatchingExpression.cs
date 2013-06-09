using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class PatternMatchingExpression : Expression
    {
        public Expression TestExpression { get; set; }

        public List<Tuple<Expression, Expression>> Patterns { get; set; }

        public PatternMatchingExpression()
        {
            Patterns = new List<Tuple<Expression, Expression>>();
        }
    }
}
