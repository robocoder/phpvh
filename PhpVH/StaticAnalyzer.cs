using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PhpVH.LexicalAnalysis;

namespace PhpVH
{
    public static class StaticAnalyzer
    {
        public static Dictionary<string, List<string>> GetSuperGlobalFieldsFromText(string text)
        {
            var r = "(" + Php.Superglobals
                .Select(x => "(" + Regex.Escape(x) + ")")
                .Aggregate((x, y) => x + "|" + y) +
                @")\[(" + Php.ValidNameRegex + @")\]";
                    
            var matches = Regex.Matches(text, r).OfType<Match>();

            var fields = new Dictionary<string, List<string>>();

            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;

                if (!fields.ContainsKey(key))
                    fields.Add(key, new List<string>());

                fields[key].Add(match.Groups[7].Value);
            }

            return fields;
        }

        public static Dictionary<string, List<string>> FindSuperglobalFields(PhpToken[] tokens)
        {
            var fields = PhpParser.GetSuperglobalFields(tokens);

            foreach (var superglobal in tokens
                .Where(x => x.TokenType == PhpTokenType.String || 
                    x.TokenType == PhpTokenType.HereDocString || 
                    x.TokenType == PhpTokenType.BacktickString)
                .Select(x => GetSuperGlobalFieldsFromText(x.Lexeme))
                .SelectMany(x => x))
                fields[superglobal.Key].AddRange(superglobal.Value);

            return fields;
        }
    }
}
