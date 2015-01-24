using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;
using Components;

namespace PhpVH.StaticAnalysis
{
    public abstract class InsecureCallPlugin : StaticAnalysisPlugin
    {
        protected abstract IEnumerable<string> GetFunctions();

        protected virtual bool IsCallInsecure(FunctionCall call)
        {
            return 
                call.ParamTokens
                    .Any(y => y.TokenType == PhpTokenType.Variable && PhpName.Superglobals
                        .Any(z => z == y.Lexeme)) ||
                call.ParamTokens
                    .Any(y => (y.TokenType == PhpTokenType.String || y.TokenType == PhpTokenType.HereDocString) &&
                        PhpName.Superglobals.Any(z => y.Lexeme.Contains(z)));
        }

        protected virtual string[] GetSanitizationFunctions()
        {
            return new string[0];
        }

        public override StaticAnalysisAlert[] GetAlerts(string code, PhpToken[] tokens)
        {
            var functions = PhpParser.GetGlobalFunctionCalls(tokens);

            var sanitizationFunctions = GetSanitizationFunctions();
            var targetFunctionNames = GetFunctions();
            var targetFunctionCalls = functions
                .Where(x => targetFunctionNames.Contains(x.Id.Lexeme) &&
                    !x.ParamTokens.Any(y => sanitizationFunctions.Contains(y.Lexeme)));
            var insecureCalls = targetFunctionCalls.Where(IsCallInsecure);

            return insecureCalls
                .Select(x => 
                {
                    var line = code.GetLineNumber(x.Id.Index);
                    var code2 = code.InsertLineNumbers();
                    var index = code2.GetLineIndex(line);
                    
                    return new StaticAnalysisAlert(Name, line + 1,
                        code2.GetSurroundingLines(index, 9, 9));
                })
                .ToArray();
        }
    }
}
