using Components.Aphid.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class AphidParserException : Exception
    {
        public AphidToken Token { get; set; }

        public AphidParserException(AphidToken token)
        {
            Token = token;
        }
    }
}
