using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;

namespace PhpVH.StaticAnalysis
{
    public class CommandInjectionPlugin2 : StaticAnalysisPlugin
    {
        protected override string Name
        {
            get { return "Command injection"; }
        }

        public override StaticAnalysisAlert[] GetAlerts(string code, PhpToken[] tokens)
        {
            return tokens
                .Where(x =>
                    x.TokenType == PhpTokenType.BacktickString &&
                    PhpName.Superglobals
                        .Any(y => x.Lexeme.Contains(y)))
                .Select(x => CreateAlert(code, x))
                .ToArray();
            
        }
    }
}
