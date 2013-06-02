using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Components;
using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public class ExpressionWalker
    {
        private PhpToken[] _tokens;

        public List<ExpressionAnalyzer> Analyzers { get; private set; }

        public ExpressionWalker(PhpToken[] tokens, params ExpressionAnalyzer[] analyzers)
        {
            Analyzers = new List<ExpressionAnalyzer>();

            if (analyzers != null)
            {
                Analyzers.AddRange(analyzers);
            }

            _tokens = tokens;
        }

        public void Walk()
        {
            _tokens.Iter(x => Analyzers.Iter(y => y.Analyze(x)));
        }
    }
}
