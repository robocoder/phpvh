using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;

namespace PhpVH.StaticAnalysis
{
    public class OpenRedirectPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[] { "header" };
        }

        protected override bool IsCallInsecure(FunctionCall call)
        {
            return base.IsCallInsecure(call) && call.ParamTokens
                .Any(x => (x.TokenType == PhpTokenType.String || x.TokenType == PhpTokenType.HereDocString) &&
                    x.Lexeme.ToLower().Contains("location:") &&
                    (PhpName.Superglobals
                        .Any(y => x.Lexeme.Contains(y)) ||
                    call.ParamTokens
                        .Any(y => y.TokenType == PhpTokenType.Variable && PhpName.Superglobals
                            .Any(z => y.Lexeme.Contains(z)))));
        }

        protected override string Name
        {
            get { return "Open redirect"; }
        }
    }
}
