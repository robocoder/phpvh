using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PhpVH
{
    public static class PatchHelper
    {
        public static string PatchForeachLoops(string Code)
        {
            foreach (string name in PhpName.SuperGlobalNames)
                Code = Regex.Replace(Code, @"(foreach\s*\(\s*\$_" + name + @")(\s+as)", "$1->container$2");

            return Code;
        }

        public static string PatchArrayTyping(string Code)
        {
            return Regex.Replace(Code, @"([\(,]\s*)(array)(\s*)(\$" + Php.ValidNameRegex + @")", "$1$3$4");
        }

        public static string PatchArrayFunctions(string Code)
        {
            var tokens = PhpParser.StripWhitespaceAndComments(new PhpVH.LexicalAnalysis.PhpLexer(Code).GetTokens().ToArray());
            var arrayFuncs = tokens
                .Where((x, index) => 
                    index > 1 &&
                    x.TokenType == LexicalAnalysis.PhpTokenType.Identifier &&
                    tokens[index - 1].TokenType != LexicalAnalysis.PhpTokenType.functionKeyword &&
                    tokens[index - 2].TokenType != LexicalAnalysis.PhpTokenType.functionKeyword &&
                    tokens[index - 1].TokenType != LexicalAnalysis.PhpTokenType.ObjectOperator &&
                    tokens[index - 1].TokenType != LexicalAnalysis.PhpTokenType.ScopeResolutionOperator &&
                    PhpName.ArrayFunctions.Contains(x.Lexeme))
                .Reverse()
                .ToArray();

            var c = new StringBuilder(Code);

            foreach (var f in arrayFuncs)
            {
                c.Insert(f.Index + f.Lexeme.Length, "_override");
            }

            return c.ToString();
        }

        public static string PatchSuperGlobalConcat(string Code)
        {
            var g = string.Join("|", PhpName.SuperGlobalNames);
            var r = @"(\$_(" + g + @"))\s*\+\s*(\$_(" + g + @"))\s*;";
            var matches = Regex.Matches(Code, r).OfType<Match>().Reverse();
            
            foreach (var m in matches)
            {
                var replacement =
                    m.Groups[1].Value + "->container" +
                    " + " +
                    m.Groups[3].Value + "->container;";

                Code = Code
                    .Remove(m.Index, m.Length)
                    .Insert(m.Index, replacement);
            }

            return Code;
        }

        public static string Patch(string Code)
        {
            return PatchSuperGlobalConcat(PatchArrayTyping(PatchForeachLoops(PatchArrayFunctions(Code))));
        }
    }
}
