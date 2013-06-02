using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AphidCodeGenerator
{
    public class AphidParserRules
    {
        [AphidProperty("binOpExps")]
        public AphidBinaryOperatorExpressionRule[] BinaryOperatorExpressions { get; set; }
    }    
}
