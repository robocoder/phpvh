using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Parser
{
    public class FunctionExpression : Expression
    {
        public List<IdentifierExpression> Args { get; set; }

        public List<Expression> Body { get; set; }

        public override string ToString ()
        {
            return string.Format ("@({0}) {{ {1} }}", 
                Args.Select(x => x.ToString()).DefaultIfEmpty().Aggregate((x, y) => x + " " + y), 
                Body.Select(x => x.ToString()).DefaultIfEmpty().Aggregate((x, y) => x + " " + y));
        }
    }
}
