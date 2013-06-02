using Components.Aphid.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Parser
{
    public class UnaryOperatorExpression : Expression
    {
        public AphidTokenType Operator { get; set; }

        public Expression Operand { get; set; }

        public bool IsPostfix { get; set; }

        public UnaryOperatorExpression(AphidTokenType op, Expression operand)
        {
            Operator = op;
            Operand = operand;
        }

        public override string ToString ()
        {
            return IsPostfix ? 
                string.Format ("{0} {1}", Operand, Operator) :
                    string.Format ("{0} {1}", Operator, Operand);
        }
    }
}
