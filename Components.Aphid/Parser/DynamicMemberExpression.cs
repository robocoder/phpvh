using System;

namespace Components.Aphid.Parser
{
    public class DynamicMemberExpression : Expression
    {
        public Expression MemberExpression { get; set; }

        public DynamicMemberExpression ()
        {
        }

        public DynamicMemberExpression (Expression memberExpression)
        {
            MemberExpression = memberExpression;
        }
    }
}

