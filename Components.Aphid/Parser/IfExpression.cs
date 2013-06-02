using System;
using System.Collections.Generic;
using Components.Aphid.Lexer;

namespace Components.Aphid.Parser
{
    public class IfExpression : ControlFlowExpression
    {
        public List<Expression> ElseBody { get; set; }

        public IfExpression(Expression condition, List<Expression> body, List<Expression> elseBody)
            : base(AphidTokenType.ifKeyword, condition, body)
        {
            ElseBody = elseBody;
        }
    }
}

