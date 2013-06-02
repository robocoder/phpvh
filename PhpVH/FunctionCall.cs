using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;

namespace PhpVH
{
    public struct FunctionCall
    {
        public PhpToken Id;

        public PhpToken[] ParamTokens;

        public FunctionCall(PhpToken id, PhpToken[] parameters)
        {
            Id = id;
            ParamTokens = parameters;
        }
    }
}
