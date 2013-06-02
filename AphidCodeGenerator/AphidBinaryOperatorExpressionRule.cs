using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AphidCodeGenerator
{
    public class AphidBinaryOperatorExpressionRule
    {
        [AphidProperty("name")]
        public string Name { get; set; }

        [AphidProperty("operators")]
        public string[] Operators { get; set; }

        [AphidProperty("operand")]
        public string Operand { get; set; }
    }
}
