using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public class Expression
    {
        public List<PhpToken> Tokens { get; private set; }

        public Expression()
        {
            Tokens = new List<PhpToken>();
        }
    }
}
