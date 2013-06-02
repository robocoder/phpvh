using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class StringExpression : ValueExpression<string>
    {
        public StringExpression(string value)
            : base(value)
        {
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
