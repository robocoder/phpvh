using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    [TestFixture]
    public class LexerHereDocTest
    {
        [Test, Category("PhpLexer")]
        public void Three_Less_Thans_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.Variable, "$test" },
                { PhpTokenType.AssignmentOperator, "=" },
                { PhpTokenType.HereDocString, "<<<EOT\nEOT" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("PhpLexer")]
        public void HereDoc_With_Other_Tokens_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.HereDocString, "<<<EOT\n//@\n'test'\"value\"<<<\nEOT" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("PhpLexer")]
        public void HereDoc_Single_Quoted_Terminal_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.HereDocString, "<<<'_EOT_'\ntest\n_EOT_" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        /*[Test, Category("PhpLexer")]
        [Ignore("\"I've never hit it in the wild, heredocs are quite rare, and it's going to be a pain,\" said John Leitch.")]
        public void HereDoc_Double_Quoted_Terminal_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { TokenType.OpenTag, "<?" },
                { TokenType.HereDocString, "<<<\"_EOT_\"\ntest\n_EOT_" },
                { TokenType.WhiteSpace, "\n" },
                { TokenType.CloseTag, "?>" }
            });
        }*/

        [Test, Category("PhpLexer")]
        public void HereDoc_With_Inner_HereDoc_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.HereDocString, "<<<_EOT_\n<<<\n_EOT_" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("PhpLexer")]
        public void HereDoc_With_Extra_Whitespace_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.HereDocString, "<<<\t \t _EOT_ \t \t\n<<<\n_EOT_" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("PhpLexer")]
        public void HereDoc_In_Arguments_Returns_HereDocString_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.Identifier, "var_dump" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.arrayKeyword, "array" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.HereDocString, "<<<EOD\nfoobar!\nEOD" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }
    }
}
