using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public class SwitchStatementAnalyzer : ExpressionAnalyzer
    {
        protected override void AnalyzeCore(PhpToken token)
        {
            if (token.TokenType == PhpTokenType.WhiteSpace ||
                token.TokenType == PhpTokenType.Comment)
            {
                return;
            }
            else if (State == 0 && token.TokenType == PhpTokenType.switchKeyword)
            {
                State++;
            }
            else if (State == 1 && token.TokenType == PhpTokenType.LeftParenthesis)
            {
                State++;
            }
            else if (State == 2 &&
                token.TokenType == PhpTokenType.Variable &&
                Php.Superglobals.Contains(token.Lexeme))
            {
                AddExpressionToken();
            }
            else if (State == 3 && token.TokenType == PhpTokenType.LeftBracket)
            {
                State++;
            }
            else if (State == 4 && token.TokenType == PhpTokenType.String)
            {
                AddExpressionToken();
            }
            else if (State == 5 && token.TokenType == PhpTokenType.RightBracket)
            {
                State++;
            }
            else if (State == 6 && token.TokenType == PhpTokenType.RightParenthesis)
            {
                State++;
            }
            else if (State == 7)
            {
                if (token.TokenType == PhpTokenType.RightBrace)
                {
                    AddExpression();
                }
                else if (token.TokenType == PhpTokenType.caseKeyword)
                {
                    State++;
                }
            }
            else if (State == 8)
            {
                if (token.TokenType == PhpTokenType.String)
                {
                    AddExpressionToken();
                }

                State = 7;
            }
            else
            {
                NewExpression();
            }
        }
    }
}
