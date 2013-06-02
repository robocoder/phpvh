using System.Collections.Generic;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    [TestFixture]
    public class LexerCommentTest
    {
        [Test, Category("PhpLexer")]
        public void PHP_Supports_C_And_C_Plus_Plus_And_Perl_Style_Comments()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.String, "'This is a test'" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Comment, "// This is a one-line c++ style comment" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "/*  This is a multi line comment\n       yet another line of comment */" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.String, "'This is yet another test'" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.String, "'One Final Test'" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Comment, "# This is a one-line shell-style comment" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" },
            });
        }

        [Test, Category("PhpLexer")]
        public void One_Line_Html_Php_Mixed_With_Comment_Returns_Tokens()
        {
            var lexer = new PhpLexer("<h1>This is an <?php # echo 'simple';?> example</h1>");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.Html, "<h1>This is an ", 0),
                new PhpToken(PhpTokenType.OpenTag, "<?php", 15),
                new PhpToken(PhpTokenType.WhiteSpace, " ", 20),
                new PhpToken(PhpTokenType.Comment, "# echo 'simple';", 21),
                new PhpToken(PhpTokenType.CloseTag, "?>", 37),
                new PhpToken(PhpTokenType.Html, " example</h1>", 39)
            }, tokens);
        }

        [Test, Category("PhpLexer")]
        public void Toggle_Comments_Returns_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "//*" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.ifKeyword, "if" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Variable, "$foo" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftBrace, "{" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, "\t" },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Variable, "$bar" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.RightBrace, "}" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "// */" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Identifier, "sort" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Variable, "$morecode" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" },
                { PhpTokenType.Html, "\n" },
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "/*\nif ($foo) {\n\techo $bar;\n}\n// */" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Identifier, "sort" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Variable, "$morecode" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" },
                { PhpTokenType.Html, "\n" },
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "//*" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.ifKeyword, "if" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Variable, "$foo" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftBrace, "{" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Variable, "$bar" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.RightBrace, "}" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "/*/\nif ($bar) {\n\techo $foo;\n}\n// */" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" },
                { PhpTokenType.Html, "\n" },
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "/*\nif ($foo) {\n  echo $bar;\n}\n/*/" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.ifKeyword, "if" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Variable, "$bar" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.LeftBrace, "{" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "echo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Variable, "$foo" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.RightBrace, "}" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.Comment, "// */" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("PhpLexer")]
        public void Two_Slashes_Returns_Comment_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Comment, "//");
        }

        [Test, Category("PhpLexer")]
        public void Three_Slashes_Returns_Comment_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Comment, "///");
        }

        [Test, Category("PhpLexer")]
        public void Star_Slash_Returns_Comment_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Comment, "/* comment */");
        }

        [Test, Category("PhpLexer")]
        public void Star_Slash_No_Close_Tag_Returns_Comment_Token()
        {
            var lexer = new PhpLexer("<?/*");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.OpenTag, "<?", 0),
                new PhpToken(PhpTokenType.Comment, "/*", 2)
            }, tokens);
        }

        [Test, Category("PhpLexer")]
        public void Two_Slashes_Newline_Returns_Comment_Token()
        {
            var lexer = new PhpLexer("<?//\nfunct");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.OpenTag, "<?", 0),
                new PhpToken(PhpTokenType.Comment, "//", 2),
                new PhpToken(PhpTokenType.WhiteSpace, "\n", 4),
                new PhpToken(PhpTokenType.Unknown, "funct", 5)
            }, tokens);
        }

        [Test, Category("PhpLexer")]
        public void Two_Slashes_No_Close_Tag_Returns_Comment_Token()
        {
            var lexer = new PhpLexer("<?//");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.OpenTag, "<?", 0),
                new PhpToken(PhpTokenType.Comment, "//", 2)
            }, tokens);
        }
    }
}
