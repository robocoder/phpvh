using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PhpVH
{
    public static class PreloadHelper
    {
        public static string[] Preloaded = new string[]
	    {
		    "GET",
		    "POST",
		    "REQUEST",
		    "COOKIE"
	    };

        public static string[] ArrayFunction = new string[]
	    {
            "is_array",
		    "array_change_key_case",
            "array_chunk",
            "array_combine",
            "array_count_values",
            "array_diff_assoc",
            "array_diff_key",
            "array_diff_uassoc",
            "array_diff_ukey",
            "array_diff",
            "array_fill_keys",
            "array_fill",
            "array_filter",
            "array_flip",
            "array_intersect_assoc",
            "array_intersect_key",
            "array_intersect_uassoc",
            "array_intersect_ukey",
            "array_intersect",
            "array_key_exists",
            "array_keys",
            "array_map",
            "array_merge_recursive",
            "array_merge",
            "array_multisort",
            "array_pad",
            "array_pop",
            "array_product",
            "array_push",
            "array_rand",
            "array_reduce",
            "array_replace_recursive",
            "array_replace",
            "array_reverse",
            "array_search",
            "array_shift",
            "array_slice",
            "array_splice",
            "array_sum",
            "array_udiff_assoc",
            "array_udiff_uassoc",
            "array_udiff",
            "array_uintersect_assoc",
            "array_uintersect_uassoc",
            "array_uintersect",
            "array_unique",
            "array_unshift",
            "array_values",
            "array_walk_recursive",
            "array_walk",
            //"array",
            "arsort",
            "asort",
            "compact",
            "count",
            "current",
            "each",
            "end",
            "extract",
            "in_array",
            "key",
            "krsort",
            "ksort",
            //"list",
            "natcasesort",
            "natsort",
            "next",
            "pos",
            "prev",
            "range",
            "reset",
            "rsort",
            "shuffle",
            "sizeof",
            "sort",
            "uasort",
            "uksort",
            "usort"
	    };

        public static string PatchForeachLoops(string Code)
        {
            foreach (string name in Preloaded)
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
                    ArrayFunction.Contains(x.Lexeme))
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
