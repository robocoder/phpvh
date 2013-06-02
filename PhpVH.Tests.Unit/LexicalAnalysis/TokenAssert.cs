using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    public static class TokenAssert
    {
        public static void IsValid(TokenPairs pairs)
        {
            var text = string.Join("", pairs.Select(pair => pair.Value).ToArray());
            var lexer = new PhpLexer(text);
            int index = 0;
            var expected = pairs.Select(pair =>
            {
                var token = new PhpToken(pair.Key, pair.Value, index);
                index += pair.Value.Length;
                return token;
            });
            List<PhpToken> actual = lexer.GetTokens();
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void IsValid(PhpTokenType expectedTokenType, string actualText)
        {
            IsValid(new TokenPairs {
               { PhpTokenType.OpenTag, "<?" },
               { PhpTokenType.WhiteSpace, " " },
               { expectedTokenType, actualText },
               { PhpTokenType.CloseTag, "?>" }
            });
        }

        public static void AreHtmlTokens(string[] texts)
        {
            foreach (string text in texts)
                IsValid(new TokenPairs { { PhpTokenType.Html, text } });
        }
    }
}
