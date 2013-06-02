using Components;
using System.Collections.Generic;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    [TestFixture, Category("PhpLexer")]
    public partial class LexerTest
    {
        [Test]
        public void Default_Constructor_Returns_Empty_List()
        {
            var lexer = new PhpLexer();
            LexerAssert.IsEmpty(lexer);
            Assert.IsNullOrEmpty(lexer.Text);
        }

        [Test]
        public void Empty_String_Returns_Empty_List()
        {
            var lexer = new PhpLexer("");
            LexerAssert.IsEmpty(lexer);
            Assert.IsNullOrEmpty(lexer.Text);
        }

        [Test]
        public void Simple_Open_Tag_Returns_OpenTag_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" }
            });
        }

        [Test]
        public void Standard_Open_Tag_Returns_OpenTag_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?php" }
            });
        }

        [Test]
        public void Simple_Open_And_Close_Tags_Returns_OpenTag_And_CloseTag_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void Standard_Open_And_Close_Tags_Returns_OpenTag_And_CloseTag_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?php" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void LessThan_QuestionMark_phz_Returns_OpenTag_And_Unknown_Tokens()
        {
            var lexer = new PhpLexer("<?phz");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.OpenTag, "<?", 0),
                new PhpToken(PhpTokenType.Unknown, "phz", 2)
            }, tokens);
        }

        [Test]
        public void Simple_Open_Tag_And_Echo_Returns_OpenTag_And_Echo_Tokens()
        {
            var lexer = new PhpLexer("<?echo(\"test\")");
            var tokens = lexer.GetTokens();
            CollectionAssert.AreEqual(new List<PhpToken>
            {
                new PhpToken(PhpTokenType.OpenTag, "<?", 0),
                new PhpToken(PhpTokenType.Identifier, "echo", 2),
                new PhpToken(PhpTokenType.LeftParenthesis, "(", 6),
                new PhpToken(PhpTokenType.String, "\"test\"", 7),
                new PhpToken(PhpTokenType.RightParenthesis, ")", 13)
            }, tokens);
        }

        [Test]
        public void LessThan_QuestionMark_Equals_Returns_OpenTagWithEcho_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTagWithEcho, "<?=" }
            });
        }

        [Test]
        public void Asp_Open_Tag_Returns_OpenTag_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<%" }
            });
        }

        [Test]
        public void Asp_Open_Tag_Equals_Returns_OpenTagWithEcho_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTagWithEcho, "<%=" }
            });
        }

        [Test]
        public void Asp_Open_And_Close_Tags_Returns_OpenTag_And_CloseTag_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<%" },
                { PhpTokenType.CloseTag, "%>" }
            });
        }

        [Test]
        public void Simple_Open_And_Close_Tags_With_Variable_Returns_OpenTagWithEcho_And_Variable_And_CloseTag_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTagWithEcho, "<%=" },
                { PhpTokenType.Variable, "$variable" },
                { PhpTokenType.CloseTag, "%>" }
            });
        }

        [Test]
        public void Bad_Open_Tag_Returns_Html_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.Html, "<=$variable?>" }
            });
        }

        [Test]
        public void First_Character_Isnt_Less_Than_Returns_Html_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.Html, "echo(\"test\");" }
            });
        }

        [Test]
        public void Plus_Sign_Returns_AdditionOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.AdditionOperator, "+");
        }

        [Test]
        public void Minus_Sign_Returns_MinusOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.MinusOperator, "-");
        }

        [Test]
        public void Ampersand_Returns_Ampersand_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Ampersand, "&");
        }

        [Test]
        public void Two_Ampersands_Returns_AndOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.AndOperator, "&&");
        }

        [Test]
        public void Equals_Sign_Returns_AssignmentOperator_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.AssignmentOperator, "=" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void Two_Backticks_Returns_BacktickString_Token()
        {
            TokenAssert.IsValid(PhpTokenType.BacktickString, "``");
        }

        [Test]
        public void Comma_Returns_Comma_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Comma, ",");
        }

        [Test]
        public void Tilde_Returns_ComplementOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ComplementOperator, "~");
        }

        [Test]
        public void Period_Equals_Returns_ConcatEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ConcatEqualOperator, ".=");
        }

        [Test]
        public void Two_Minuses_Returns_Decrement_Operator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.DecrementOperator, "--");
        }

        [Test]
        public void Forward_Slash_Equals_Returns_DivisonEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.DivisionEqualOperator, "/=");
        }

        [Test]
        public void Forward_Slash_Returns_DivisonOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.DivisionOperator, "/");
        }

        // TODO: Test EndOfFile

        [Test]
        public void Semicolon_Returns_EndOfStatement_Token()
        {
            TokenAssert.IsValid(PhpTokenType.EndOfStatement, ";");
        }

        [Test]
        public void Equals_Equals_Returns_EqualityOperator_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.EqualityOperator, "==" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void At_Symbol_Returns_ErrorSuppressor_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ErrorSuppressor, "@");
        }

        [Test]
        public void Greater_Than_Returns_GreaterThanOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.GreaterThanOperator, ">");
        }

        [Test]
        public void Greater_Than_Equals_Returns_GreaterThanOrEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.GreaterThanOrEqualOperator, ">=");
        }

        [Test]
        public void All_Tokens_Not_In_PHP_Open_Close_Tags_Returns_Html_Token()
        {
            TokenAssert.AreHtmlTokens(new[] {
                "+", "&", "&&", "=", "``", ",", "//", "///", "/* comment */", "~",
                ".=", "--", "/=", "/", ";", "==", "@", ">", ">=", "===", "-"
            });
        }

        [Test]
        public void Equals_Equals_Equals_Returns_IdenticalOperator_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.IdenticalOperator, "===" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void Word_test_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "test");
        }

        [Test]
        public void PlusSign_PlusSign_Returns_IncrementOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.IncrementOperator, "++");
        }

        [Test]
        public void Left_Brace_Returns_LeftBrace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.LeftBrace, "{");
        }

        [Test]
        public void Left_Bracket_Returns_LeftBracket_Token()
        {
            TokenAssert.IsValid(PhpTokenType.LeftBracket, "[");
        }

        [Test]
        public void Left_Parenthesis_Returns_LeftParenthesis_Token()
        {
            TokenAssert.IsValid(PhpTokenType.LeftParenthesis, "(");
        }

        [Test]
        public void Less_Than_Returns_LessThanOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.LessThanOperator, "<");
        }

        [Test]
        public void Less_Than_Equals_Returns_LessThanOrEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.LessThanOrEqualOperator, "<=");
        }

        [Test]
        public void Minus_Equals_Returns_MinusEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.MinusEqualOperator, "-=");
        }

        [Test]
        public void Modulus_Equals_Returns_ModulusEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ModulusEqualOperator, "%=");
        }

        [Test]
        public void Modulus_Returns_ModulusOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ModulusOperator, "%");
        }

        [Test]
        public void Star_Equals_Returns_MultiplicationEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.MultiplicationEqualOperator, "*=");
        }

        [Test]
        public void Star_Returns_MultiplicationOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.MultiplicationOperator, "*");
        }

        [Test]
        public void Two_Backslashes_Returns_Namespace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Namespace, "\\");
        }

        [Test]
        public void Exclamation_Equals_Returns_NotEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.NotEqualOperator, "!=");
        }

        [Test]
        public void Exclamation_Equals_Equals_Returns_NotIdenticalOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.NotIdenticalOperator, "!==");
        }

        [Test]
        public void Exclamation_Returns_NotOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.NotOperator, "!");
        }

        [Test]
        public void Zero_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "0");
        }

        [Test]
        public void Zero_One_Two_Three_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "0123");
        }

        [Test]
        public void One_Two_Three_Four_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1234");
        }

        //[Test]
        //public void Negative_One_Two_Three_Four_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1234");
        //}

        [Test]
        public void One_Point_Two_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1.2");
        }

        //[Test]
        //public void Negative_One_Point_Two_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1.2");
        //}

        [Test]
        public void One_Two_Point_Three_Four_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "12.34");
        }

        //[Test]
        //public void Negative_One_Two_Point_Three_Four_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-12.34");
        //}

        [Test]
        public void One_Point_Two_Three_Four_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1.234");
        }

        //[Test]
        //public void Negative_One_Point_Two_Three_Four_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1.234");
        //}

        [Test]
        public void Scientific_Notation_One_e_Four_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1e4");
        }

        //[Test]
        //public void Scientific_Notation_Negative_One_e_Four_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1e4");
        //}

        [Test]
        public void Scientific_Notation_One_E_Positive_Seventeen_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1E+17");
        }

        //[Test]
        //public void Scientific_Notation_Negative_One_E__Positive_Seventeen_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1E+17");
        //}

        [Test]
        public void Scientific_Notation_One_E_Negative_Seventeen_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "1E-17");
        }

        //[Test]
        //public void Scientific_Notation_Negative_One_E_Negative_Seventeen_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-1E-17");
        //}

        [Test]
        public void Scientific_Notation_One_Two_Point_Three_Four_e_Positive_Seventeen_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number, "12.34e+17");
        }

        //[Test]
        //public void Scientific_Notation_Negative_One_Two_Point_Three_Four_E_Negative_Seventeen_Returns_Number_Token()
        //{
        //    TokenAssert.IsValid(PhpTokenType.Number, "-12.34E-17");
        //}

        [Test]
        public void Infinity_Number_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Number,
                "5863527282178658728638282465756887109828727827654321987654313467543856894567354783568567456845674567" +
                "4568456879567967806789567835672457356845681754328057895676305123516234516879145256946795686296754296" +
                "8541234634578356854683456735423475469675723465689456825624590764786234635685673456283456895645687456" +
                "8923745902834758902347590234652908357902384759023847590238457092384572345908273459087234590872345234");
        }

        [Test]
        public void Hexadecimal_Number_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.HexNumber, "0xDEADBEEF");
        }

        [Test]
        public void Mixed_Case_Hexadecimal_Number_Returns_Number_Token()
        {
            TokenAssert.IsValid(PhpTokenType.HexNumber, "0xaaAAffFF");
        }

        [Test]
        public void Incomplete_Hexadecimal_Number_Returns_Unknown_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Unknown, "0x");
        }

        [Test]
        public void Invalid_Hexadecimal_Number_Returns_Unknown_Token()
        {
            new string[] { "g", "G", }.Iter(x =>
                TokenAssert.IsValid(new TokenPairs
                {
                    { PhpTokenType.OpenTag, "<?"},
                    { PhpTokenType.WhiteSpace, " " },
                    { PhpTokenType.Unknown, "0x" },
                    { PhpTokenType.Identifier, x + "aaAAffFF" },
                    { PhpTokenType.WhiteSpace, " " },
                    { PhpTokenType.CloseTag, "?>"},
                }));
        }
        
        [Test]
        public void Dash_Greater_Than_Returns_ObjectOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ObjectOperator, "->");
        }

        [Test]
        public void Pipe_Equals_Returns_OrEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.OrEqualOperator, "|=");
        }

        [Test]
        public void Two_Pipes_Returns_OrOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.OrOperator, "||");
        }

        [Test]
        public void Pipe_Returns_OrOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.OrOperator, "|");
        }

        [Test]
        public void Plus_Sign_Equals_Returns_PlusEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.PlusEqualOperator, "+=");
        }

        [Test]
        public void Right_Brace_Returns_RightBrace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.RightBrace, "}");
        }

        [Test]
        public void Right_Bracket_Returns_RightBracket_Token()
        {
            TokenAssert.IsValid(PhpTokenType.RightBracket, "]");
        }

        [Test]
        public void Right_Parenthesis_Returns_RightParenthesis_Token()
        {
            TokenAssert.IsValid(PhpTokenType.RightParenthesis, ")");
        }

        [Test]
        public void Two_Semicolons_Returns_ScopeResolutionOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ScopeResolutionOperator, "::");
        }

        [Test]
        public void Two_Less_Thans_Returns_ShiftLeft_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ShiftLeft, "<<");
        }

        [Test]
        public void Two_Greater_Thans_Returns_ShiftRight_Token()
        {
            TokenAssert.IsValid(PhpTokenType.ShiftRight, ">>");
        }

        [Test]
        public void Double_Quoted_Returns_String_Token()
        {
            TokenAssert.IsValid(PhpTokenType.String, "\"value\"");
        }

        [Test]
        public void Single_Quoted_Returns_String_Token()
        {
            TokenAssert.IsValid(PhpTokenType.String, "'value'");
        }

        [Test]
        public void Period_Returns_StringConcatOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.StringConcatOperator, ".");
        }

        [Test]
        public void Question_Mark_Returns_TernaryOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.TernaryOperator, "?");
        }

        [Test]
        public void Null_Returns_Unknown_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Unknown, "\x00");
        }

        [Test]
        public void Dollar_Sign_Word_Returns_Variable_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Variable, "$variable");
        }

        [Test]
        public void Dollar_Sign_Dollar_Sign_Word_Returns_VariableVariable_Token()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.VariableVariable, "$" },
                { PhpTokenType.Variable, "$foo" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.CloseTag, "?>" },
            });
        }

        [Test]
        public void Space_Returns_WhiteSpace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.WhiteSpace, " ");
        }

        [Test]
        public void Tab_Returns_WhiteSpace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.WhiteSpace, "\t");
        }

        [Test]
        public void Backslash_N_Returns_WhiteSpace_Token()
        {
            TokenAssert.IsValid(PhpTokenType.WhiteSpace, "\n");
        }

        [Test]
        public void Simple_Open_And_Close_Tags_Around_Backslash_R_Backslash_N_Returns_WhiteSpace_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.WhiteSpace, "\r" },
                { PhpTokenType.WhiteSpace, "\n" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void Up_Carrot_Equals_Returns_XorEqualOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.XorEqualOperator, "^=");
        }

        [Test]
        public void Up_Carrot_Returns_XorOperator_Token()
        {
            TokenAssert.IsValid(PhpTokenType.XorOperator, "^");
        }

        [Test]
        public void Print_Addition_Returns_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
               { PhpTokenType.OpenTag, "<?php" },
               { PhpTokenType.WhiteSpace, " " },
               { PhpTokenType.Identifier, "print" },
               { PhpTokenType.LeftParenthesis, "(" },
               { PhpTokenType.Number, "1" },
               { PhpTokenType.AdditionOperator, "+" },
               { PhpTokenType.Number, "2" },
               { PhpTokenType.RightParenthesis, ")" },
               { PhpTokenType.WhiteSpace, " " },
               { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test]
        public void Basic_Ternary_Expression_Returns_Tokens()
        {
            TokenAssert.IsValid(new TokenPairs {
                { PhpTokenType.OpenTag, "<?" },
                { PhpTokenType.Identifier, "print" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.LeftParenthesis, "(" },
                { PhpTokenType.Identifier, "true" },
                { PhpTokenType.OrOperator, "||" },
                { PhpTokenType.Identifier, "false" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.TernaryOperator, "?" },
                { PhpTokenType.String, "'yay'" },
                { PhpTokenType.ColonOperator, ":" },
                { PhpTokenType.String, "'nay'" },
                { PhpTokenType.RightParenthesis, ")" },
                { PhpTokenType.EndOfStatement, ";" },
                { PhpTokenType.CloseTag, "?>" }
            });
        }

        [Test, Category("MaximalMunch")]
        public void Else_Returns_ElseKeyword_Token()
        {
            TokenAssert.IsValid(PhpTokenType.elseKeyword, "else");
        }

        [Test, Category("MaximalMunch")]
        public void ElseIf_Returns_ElseIfKeyword_Token()
        {
            TokenAssert.IsValid(PhpTokenType.elseifKeyword, "elseif");
        }

        [Test, Category("MaximalMunch")]
        public void E_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "e");
        }

        [Test, Category("MaximalMunch")]
        public void El_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "el");
        }

        [Test, Category("MaximalMunch")]
        public void Els_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "els");
        }

        [Test, Category("MaximalMunch")]
        public void ElseI_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "elsei");
        }

        [Test, Category("MaximalMunch")]
        public void Elsex_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "elsex");
        }

        [Test, Category("MaximalMunch")]
        public void ElseIfx_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "elseifx");
        }

        [Test]
        public void Identifier_With_Underscore_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(PhpTokenType.Identifier, "var_dump");
        }

        [Test]
        public void Left_Space_Identifier_With_Underscore_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?"},
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "var_dump" },
                { PhpTokenType.CloseTag, "?>"},
            });
        }

        [Test]
        public void Identifier_With_Underscore_Right_Space_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?"},
                { PhpTokenType.Identifier, "var_dump" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.CloseTag, "?>"},
            });
        }

        [Test]
        public void Identifier_With_Underscore_Surrounded_By_Spaces_Returns_Identifier_Token()
        {
            TokenAssert.IsValid(new TokenPairs
            {
                { PhpTokenType.OpenTag, "<?"},
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.Identifier, "var_dump" },
                { PhpTokenType.WhiteSpace, " " },
                { PhpTokenType.CloseTag, "?>"},
            });
        }
    }
}
