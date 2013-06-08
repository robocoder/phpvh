using Components.Aphid.Interpreter;
using Components.Aphid.Lexer;
using Components.Aphid.Parser;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{


    public class AphidLanguageService : LanguageService
    {
        public override string GetFormatFilterList()
        {
            return "Aphid files (*.alx)\n*.alx\n";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            return new LanguagePreferences()
            {
                EnableMatchBracesAtCaret = true,
                EnableCodeSense = true,
                CodeSenseDelay = 500,
                AutoListMembers = true,
                EnableFormatSelection = true,
                EnableMatchBraces = true,                
                LineNumbers = true,
                EnableCommenting = true,
                EnableShowMatchingBrace = true,                
                IndentStyle = IndentingStyle.Smart,
                EnableQuickInfo = true,
                MaxErrorMessages = 100,
                HighlightMatchingBraceFlags = _HighlightMatchingBraceFlags.HMB_USERECTANGLEBRACES,
                 

            };
        }

        public override IScanner GetScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer)
        {
            return new AphidScanner();
        }

        public override string Name
        {
            get { return "Aphid"; }
        }

        private void CheckParseRequest(ParseRequest req)
        {
            try
            {
                var lexer = new AphidLexer(req.Text);
                var parser = new AphidParser(lexer.GetTokens());
                parser.Parse();
            }
            catch (AphidParserException e)
            {
                var lineCol = TokenHelper.GetLineCol(req.Text, e.Token.Index);
                var span = new TextSpan()
                {
                    iStartLine = lineCol.Item1,
                    iEndLine = lineCol.Item1,
                    iStartIndex = lineCol.Item2,
                    iEndIndex = lineCol.Item2 + (e.Token.Lexeme != null ? e.Token.Lexeme.Length : 0)
                };

                var msg = string.Format("Unexpected {0}: {1}", e.Token.TokenType.ToString(), e.Token.Lexeme);

                req.Sink.AddError(req.FileName, msg, span, Severity.Error);
            }
        }

        static TextSpan CreateSpan(int line, int col)
        {
            return new TextSpan()
            {
                iStartLine = line,
                iStartIndex = col,
                iEndLine = line,
                iEndIndex = col + 1,
            };
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            switch (req.Reason)
            {
                case ParseReason.QuickInfo:
                    break;
                
                case ParseReason.DisplayMemberList:
                    break;

                case ParseReason.CompleteWord:
                    break;

                case ParseReason.MatchBraces:
                case ParseReason.MemberSelectAndHighlightBraces:
                case ParseReason.HighlightBraces:
                    var braces = TokenHelper.GetBraces(
                        req.Text, 
                        req.Line, 
                        req.Col - 1);

                    if (braces != null)
                    {

                        req.Sink.MatchPair(CreateSpan(braces[0][0], braces[0][1]), CreateSpan(braces[1][0], braces[1][1]), 1);
                    }

                    var index = TokenHelper.GetIndex(req.Text, req.Line, req.Col - 1);
                    var str = req.Text.Substring(index);
                    var tokens = new AphidLexer(str).GetAllTokens();

                    var depth = 1;
                    var rightBraceIndex = -1;
                    for (int i = 1; i < tokens.Count; i++)
                    {
                        switch (tokens[i].TokenType)
                        {
                            case AphidTokenType.LeftBrace:
                                depth++;
                                break;

                            case AphidTokenType.RightBrace:
                                depth--;
                                break;
                        }

                        if (depth == 0)
                        {
                            rightBraceIndex = index + tokens[i].Index;
                            break;
                        }
                    }

                    if (rightBraceIndex != -1)
                    {
                        var rightLineCol = TokenHelper.GetLineCol(req.Text, rightBraceIndex);

                        req.Sink.MatchPair(CreateSpan(req.Line, req.Col - 1), CreateSpan(rightLineCol.Item1, rightLineCol.Item2), 1);
                    }
                    
                    break;

                case ParseReason.Check:
                    CheckParseRequest(req);
                    break;

                default:
                    break;
            }

            return new AphidAuthoringScope();
        }
    }
}
