using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public class BooleanExpressionAnalyzer : ExpressionAnalyzer
    {
        private PhpTokenType[] _operators = new[]
        {
            PhpTokenType.EqualityOperator,
            PhpTokenType.NotEqualOperator,
            PhpTokenType.IdenticalOperator,
            PhpTokenType.NotIdenticalOperator,                
        };

        protected override void AnalyzeCore(PhpToken token)
        {
            if (token.TokenType == PhpTokenType.WhiteSpace ||
                token.TokenType == PhpTokenType.Comment)
            {
                return;
            }
            else if (State == 0 &&
                token.TokenType == PhpTokenType.Variable &&
                Php.Superglobals.Contains(token.Lexeme))
            {
                AddExpressionToken();
            }
            else if (State == 1 && token.TokenType == PhpTokenType.LeftBracket)
            {
                State++;
            }
            else if (State == 2 && token.TokenType == PhpTokenType.String)
            {
                AddExpressionToken();
            }
            else if (State == 3 && token.TokenType == PhpTokenType.RightBracket)
            {
                State++;
            }
            else if (State == 4 && _operators.Contains(token.TokenType))
            {
                AddExpressionToken();
            }
            else if (State == 5 && token.TokenType == PhpTokenType.String)
            {
                AddExpressionToken();
                AddExpression();
            }
            else
            {
                NewExpression();                
            }
        }
    }
}
