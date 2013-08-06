using Components.Aphid.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Parser
{
    public partial class AphidParser
    {
        private List<AphidToken> _tokens;

        private int _tokenIndex = -1;

        private AphidToken _currentToken;

        public AphidParser(List<AphidToken> tokens)
        {
            _tokens = tokens;
        }

        [System.Diagnostics.DebuggerStepThrough]
        private bool Match(AphidTokenType tokenType)
        {
            if (_currentToken.TokenType == tokenType)
            {
                NextToken();
                return true;
            }
            else
            {
                throw new AphidParserException(_currentToken);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        private bool NextToken()
        {
            _tokenIndex++;

            if (_tokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_tokenIndex];
                return true;
            }
            else
            {
                _currentToken = default(AphidToken);
                return false;
            }
        }

        public Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParsePostfixUnaryOperationExpression()
        {
            var term = ParseBinaryOrExpression();

            if (_currentToken.TokenType == AphidTokenType.IncrementOperator ||
                _currentToken.TokenType == AphidTokenType.ExistsOperator)
            {
                var op = _currentToken.TokenType;
                NextToken();
                return new UnaryOperatorExpression(op, term) { IsPostfix = true };
            }
            else
            {
                return term;
            }
        }

        public Expression ParsePrefixUnaryOperatorExpression()
        {
            if (_currentToken.TokenType == AphidTokenType.NotOperator)
            {
                var t = _currentToken.TokenType;
                NextToken();
                return new UnaryOperatorExpression(t, ParseArrayAccessExpression());
            }
            else
            {
                return ParseArrayAccessExpression();
            }
        }

        public Expression ParseArrayAccessExpression()
        {
            var exp = ParseCallExpression();

            while (_currentToken.TokenType == AphidTokenType.LeftBracket)
            {
                NextToken();
                var key = ParseExpression();
                Match(AphidTokenType.RightBracket);
                exp = new ArrayAccessExpression(exp, key);
            }

            return exp;
        }

        public Expression ParseCallExpression()
        {
            var function = ParseMemberExpression();

            while (_currentToken.TokenType == AphidTokenType.LeftParenthesis)
            {
                NextToken();
                if (_currentToken.TokenType == AphidTokenType.RightParenthesis)
                {
                    NextToken();
                    function = new CallExpression(function);
                }
                else
                {
                    var args = ParseTuple();
                    Match(AphidTokenType.RightParenthesis);
                    function = new CallExpression(function, args);
                }
            }

            return function;
        }

        public Expression ParseCallExpression(Expression expression)
        {
            while (_currentToken.TokenType == AphidTokenType.LeftParenthesis)
            {
                NextToken();
                if (_currentToken.TokenType == AphidTokenType.RightParenthesis)
                {
                    NextToken();
                    expression = new CallExpression(expression);
                }
                else
                {
                    var args = ParseTuple();
                    Match(AphidTokenType.RightParenthesis);
                    expression = new CallExpression(expression, args);
                }
            }

            return expression;
        }

        public Expression ParseMemberExpression()
        {
            Expression factor = ParseCallExpression(ParseFactor());

            while (_currentToken.TokenType == AphidTokenType.MemberOperator)
            {
                NextToken();

                Expression exp;

                switch (_currentToken.TokenType)
                {
                    case AphidTokenType.Identifier:
                        exp = new IdentifierExpression(_currentToken.Lexeme);
                        NextToken();
                        break;

                    case AphidTokenType.String:
                        exp = ParseStringExpression();
                        break;

                    case AphidTokenType.LeftBrace:
                        NextToken();
                        exp = new DynamicMemberExpression(ParseExpression());
                        Match(AphidTokenType.RightBrace);
                        break;

                    default:
                        throw new AphidParserException(_currentToken);
                }

                factor = ParseCallExpression(new BinaryOperatorExpression(factor, AphidTokenType.MemberOperator, exp));
            }

            return factor;
        }

        public Expression ParseFactor()
        {
            Expression exp;
            switch (_currentToken.TokenType)
            {
                case AphidTokenType.LeftBrace:
                    exp = ParseObjectExpression();
                    break;

                case AphidTokenType.LeftBracket:
                    exp = ParseArrayExpression();
                    break;

                case AphidTokenType.LeftParenthesis:
                    NextToken();
                    exp = ParseExpression();
                    Match(AphidTokenType.RightParenthesis);
                    break;

                case AphidTokenType.String:
                    exp = ParseStringExpression();
                    break;

                case AphidTokenType.Number:
                    exp = ParseNumberExpression();
                    break;

                case AphidTokenType.Identifier:
                    exp = ParseIdentifierExpression();
                    break;

                case AphidTokenType.functionOperator:
                    exp = ParseFunctionExpression();
                    break;

                case AphidTokenType.forKeyword:
                    exp = ParseForExpression();
                    break;

                case AphidTokenType.retKeyword:
                    exp = ParseReturnExpression();
                    break;

                case AphidTokenType.trueKeyword:
                    exp = new BooleanExpression(true);
                    NextToken();
                    break;

                case AphidTokenType.falseKeyword:
                    exp = new BooleanExpression(false);
                    NextToken();
                    break;

                case AphidTokenType.thisKeyword:
                    exp = new ThisExpression();
                    NextToken();
                    break;

                case AphidTokenType.ifKeyword:
                    exp = ParseIfExpression();
                    break;

                case AphidTokenType.LoadScriptOperator:
                    exp = ParseLoadScriptExpression();
                    break;

                case AphidTokenType.LoadLibraryOperator:
                    exp = ParseLoadLibraryExpression();
                    break;

                case AphidTokenType.nullKeyword:
                    exp = new NullExpression();
                    NextToken();
                    break;

                case AphidTokenType.breakKeyword:
                    exp = new BreakExpression();
                    NextToken();
                    break;

                case AphidTokenType.HexNumber:
                    exp = new NumberExpression((decimal)Convert.ToInt64(_currentToken.Lexeme.Substring(2), 16));
                    NextToken();
                    break;

                case AphidTokenType.PatternMatchingOperator:
                    var matchExp = new PatternMatchingExpression();
                    NextToken();
                    Match(AphidTokenType.LeftParenthesis);
                    matchExp.TestExpression = ParseExpression();
                    Match(AphidTokenType.RightParenthesis);

                    while (true)
                    {
                        var tests = new List<Expression>();

                        while (true)
                        {
                            tests.Add(ParseExpression());

                            if (_currentToken.TokenType == AphidTokenType.Comma)
                            {
                                NextToken();
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (_currentToken.TokenType == AphidTokenType.ColonOperator)
                        {
                            NextToken();

                            var b = ParseExpression();

                            foreach (var t in tests)
                            {
                                matchExp.Patterns.Add(new Tuple<Expression, Expression>(t, b));
                            }
                        }
                        else
                        {
                            matchExp.Patterns.Add(new Tuple<Expression,Expression>(null, tests[0]));
                        }

                        if (_currentToken.TokenType == AphidTokenType.Comma)
                        {
                            NextToken();
                        }
                        else
                        {
                            break;
                        }
                    }

                    exp = matchExp;
                    break;

                default:
                    throw new AphidParserException(_currentToken);
            }            

            return exp;
        }

        private Expression ParseFunctionExpression()
        {
            Expression exp;

            NextToken();

            switch (_currentToken.TokenType)
            {
                case AphidTokenType.LeftParenthesis:
                    var funcExp = new FunctionExpression()
                    {
                        Args = new List<IdentifierExpression>()
                    };

                    NextToken();

                    if (_currentToken.TokenType != AphidTokenType.RightParenthesis)
                    {
                        while (true)
                        {
                            if (_currentToken.TokenType == AphidTokenType.Identifier)
                            {
                                funcExp.Args.Add(ParseIdentifierExpression() as IdentifierExpression);

                                if (_currentToken.TokenType == AphidTokenType.Comma)
                                {
                                    NextToken();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                throw new AphidParserException(_currentToken);
                            }
                        }
                    }

                    Match(AphidTokenType.RightParenthesis);

                    var isSingleLine = _currentToken.TokenType != AphidTokenType.LeftBrace;

                    var body = ParseBlock(false);

                    if (isSingleLine)
                    {
                        funcExp.Body = new List<Expression> { new UnaryOperatorExpression(AphidTokenType.retKeyword, body[0]) };
                    }
                    else
                    {
                        funcExp.Body = body;
                    }

                    exp = funcExp;

                    break;

                default:

                    exp = new PartialFunctionExpression((CallExpression)ParseCallExpression());
                    break;
            }

            return exp;
        }

        private BinaryOperatorExpression ParseKeyValuePairExpression()
        {
            var id = new IdentifierExpression(_currentToken.Lexeme);
            NextToken();
            Expression exp;

            if (_currentToken.TokenType == AphidTokenType.ColonOperator)
            {
                NextToken();
                exp = ParseExpression();
            }
            else
            {
                exp = id;
            }

            return new BinaryOperatorExpression(id, AphidTokenType.ColonOperator, exp);
        }

        private StringExpression ParseStringExpression()
        {
            var exp = new StringExpression(_currentToken.Lexeme);
            NextToken();
            return exp;
        }

        private ObjectExpression ParseObjectExpression()
        {
            NextToken();

            var inNode = true;

            var childNodes = new List<BinaryOperatorExpression>();

            while (inNode)
            {
                switch (_currentToken.TokenType)
                {
                    case AphidTokenType.Identifier:
                        childNodes.Add(ParseKeyValuePairExpression());

                        switch (_currentToken.TokenType)
                        {
                            case AphidTokenType.Comma:
                                NextToken();
                                break;

                            case AphidTokenType.RightBrace:
                                NextToken();
                                inNode = false;
                                break;

                            default:
                                throw new AphidParserException(_currentToken);
                        }

                        break;

                    case AphidTokenType.RightBrace: // empty object
                        NextToken();
                        inNode = false;
                        break;

                    default:
                        throw new AphidParserException(_currentToken);
                }
            }

            return new ObjectExpression() { Pairs = childNodes };
        }

        public NumberExpression ParseNumberExpression()
        {
            var exp = new NumberExpression(decimal.Parse(_currentToken.Lexeme));
            NextToken();
            return exp;
        }

        public ArrayExpression ParseArrayExpression()
        {
            NextToken();

            var inNode = true;

            var childNodes = new List<Expression>();

            if (_currentToken.TokenType != AphidTokenType.RightBracket)
            {
                while (inNode)
                {
                    childNodes.Add(ParseExpression());

                    switch (_currentToken.TokenType)
                    {
                        case AphidTokenType.Comma:
                            NextToken();

                            if (_currentToken.TokenType == AphidTokenType.RightBracket)
                            {
                                NextToken();
                                inNode = false;
                            }

                            break;

                        case AphidTokenType.RightBracket:
                            NextToken();
                            inNode = false;
                            break;

                        default:
                            throw new AphidParserException(_currentToken);
                    }
                }
            }
            else
            {
                NextToken();
            }

            return new ArrayExpression() { Elements = childNodes };
        }

        private Expression ParseIdentifierExpression()
        {
            var exp = new IdentifierExpression(_currentToken.Lexeme);
            NextToken();
            return exp;
        }

        public UnaryOperatorExpression ParseReturnExpression()
        {
            var t = _currentToken.TokenType;
            NextToken();
            return new UnaryOperatorExpression(t, ParseExpression());
        }

        private Expression ParseCondition()
        {
            Match(AphidTokenType.LeftParenthesis);
            var condition = ParseExpression();
            Match(AphidTokenType.RightParenthesis);
            return condition;
        }

        public IfExpression ParseIfExpression()
        {
            NextToken();
            var condition = ParseCondition();
            var body = ParseBlock();
            List<Expression> elseBody = null;
            if (_currentToken.TokenType == AphidTokenType.elseKeyword)
            {
                NextToken();
                elseBody = ParseBlock();
            }
            return new IfExpression(condition, body, elseBody);
        }

        public Expression ParseForExpression()
        {
            NextToken();
            Match(AphidTokenType.LeftParenthesis);
            var initOrElement = ParseExpression();

            if (_currentToken.TokenType == AphidTokenType.inKeyword)
            {
                NextToken();
                var collection = ParseExpression();
                Match(AphidTokenType.RightParenthesis);
                var body = ParseBlock();
                return new ForEachExpression(collection, initOrElement, body);
            }
            else
            {
                Match(AphidTokenType.EndOfStatement);
                var condition = ParseExpression();
                Match(AphidTokenType.EndOfStatement);
                var afterthought = ParseExpression();
                Match(AphidTokenType.RightParenthesis);
                var body = ParseBlock();
                return new ForExpression(initOrElement, condition, afterthought, body);
            }
        }

        private List<Expression> ParseTuple()
        {
            var tuple = new List<Expression>();

            while (true)
            {
                tuple.Add(ParseExpression());

                if (_currentToken.TokenType == AphidTokenType.Comma)
                {
                    NextToken();
                }
                else
                {
                    return tuple;
                }
            }
        }

        private List<Expression> ParseBlock(bool requireSingleExpEos = true)
        {
            var statements = new List<Expression>();

            if (_currentToken.TokenType == AphidTokenType.LeftBrace)
            {
                NextToken();

                while (_currentToken.TokenType != AphidTokenType.RightBrace)
                {
                    statements.Add(ParseStatement());
                }

                NextToken();
            }
            else
            {
                statements.Add(ParseStatement(requireSingleExpEos));

                //if (requireSingleExpEos)
                //{
                //    Match(AphidTokenType.EndOfStatement);
                //}
            }

            return statements;
        }

        private LoadScriptExpression ParseLoadScriptExpression()
        {
            NextToken();
            return new LoadScriptExpression(ParseExpression());
        }

        private LoadLibraryExpression ParseLoadLibraryExpression()
        {
            NextToken();
            return new LoadLibraryExpression(ParseExpression());
        }

        private Expression ParseStatement(bool requireEos = true)
        {
            var exp = ParseExpression();

            if (requireEos &&
                !(exp is IfExpression) &&
                !(exp is ForEachExpression) &&
                !(exp is ForExpression))
            {
                Match(AphidTokenType.EndOfStatement);
            }

            return exp;
        }

        public List<Expression> Parse()
        {
            var expressionSequence = new List<Expression>();
            NextToken();

            while (_currentToken.Lexeme != null)
            {
                expressionSequence.Add(ParseStatement());
            }

            return expressionSequence;
        }
    }
}
