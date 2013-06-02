using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Components;
using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public abstract class ExpressionAnalyzer
    {
        public int State { get; protected set; }

        private PhpToken _currentToken;

        public List<Expression> Matches { get; protected set; }

        public Expression CurrentExpression { get; private set; }

        public void AddExpressionToken()
        {
            CurrentExpression.Tokens.Add(_currentToken);
            State++;
        }

        public void NewExpression()
        {
            CurrentExpression = new Expression();
            State = 0;
        }

        public void AddExpression()
        {
            Matches.Add(CurrentExpression);
            CurrentExpression = new Expression();
            State = 0;
        }

        public void Analyze(PhpToken token)
        {
            _currentToken = token;
            AnalyzeCore(token);
        }

        protected abstract void AnalyzeCore(PhpToken token);

        public ExpressionAnalyzer()
        {
            CurrentExpression = new Expression();
            Matches = new List<Expression>();
        }
    }
}
