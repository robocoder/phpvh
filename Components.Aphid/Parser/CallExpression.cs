using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Components.Aphid.Parser
{
    public class CallExpression : Expression
    {
        public Expression FunctionExpression { get; set; }

        public IEnumerable<Expression> Args { get; set; }

        public CallExpression(Expression functionExpression, IEnumerable<Expression> args)
        {
            FunctionExpression = functionExpression;
            Args = args;
        }

        public CallExpression(Expression functionExpression)
            : this (functionExpression, new Expression[0])
        {
            
        }

        public CallExpression(Expression functionExpression, Expression expression)
            : this(functionExpression, new[] { expression })
        {
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", FunctionExpression, Args.Select(x => x.ToString()).DefaultIfEmpty().Aggregate((x, y) => x + ", " + y));
        }
    }
}
